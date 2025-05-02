using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [SerializeField] private GameObject bossAttackPrefab; 
    [SerializeField] private GameObject catPrefab; 
    [SerializeField] private int hp = 50; 
    private GameObject player;
    
    private Rigidbody2D _rb;
    private Animator _an;
    private SpriteRenderer _sr;
    
    private enum Action { Idle, Walk, Cleave, Smash, FireBreath }
    private bool direction = false; // true = right 
    private int hpMax;
    private bool _isDead;
    private Image bossLiveBar;
    private Image bossLive;
    
    private void Start()
    {
        player = GameObject.Find("Player");
        bossLiveBar = GameObject.Find("BossLiveBar").GetComponent<Image>();
        bossLiveBar.fillAmount = 1f;
        bossLiveBar.color = new Color(1f, 0f, 0f, 1f);
        
        bossLive = GameObject.Find("BossLive").GetComponent<Image>();
        bossLive.color = new Color(1f, 0f, 0f, 1f);
        hpMax = hp;

        _rb = GetComponent<Rigidbody2D>();
        _an = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        direction = player.transform.position.x > transform.position.x;
        _sr.flipX = direction;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isDead) return;
        
        if (collision.gameObject.CompareTag("Weapon"))
        {
            hp--;
            bossLiveBar.fillAmount = (float) hp / hpMax;
            if (hp <= 0)
            {
                _an.SetTrigger("death");
                _rb.linearVelocity = Vector2.zero;
                _isDead = true;
            }
            else
            {
                _an.SetTrigger("damage");    
            }
        }
    }
    
    public void ResetDamage()
    {
        _an.ResetTrigger("damage");
    }

    
    public void ChoseNextAction()
    {
        if (Random.Range(0, 1) == 1)
        {
            _an.SetInteger("action", (int)Action.Walk);
            return;
        } 
        _an.SetInteger("action", Random.Range(0, System.Enum.GetValues(typeof(Action)).Length));
    }

    public void Walk()
    {
        _rb.linearVelocity = new Vector2(direction ? 6f : -6f, 0f);
    }
    
    
    public void StopWalk()
    {
        _rb.linearVelocity = new Vector2(0f, 0f);
    }
    
    public void Cleave()
    {
        GameObject bossAttack = Instantiate(bossAttackPrefab, _rb.position, quaternion.identity);

        float shift;
        if (direction)
        {
            shift = 4.2f;
        }
        else
        {
            shift = -4.2f;
        }
        bossAttack.transform.position = new Vector2(_rb.position.x + shift, _rb.position.y);
        bossAttack.transform.localScale = new Vector3(3.5f, 3f, 1f);

        StartCoroutine(DestroyObject(0.1f, bossAttack));
    }
    
    public void FireBreath()
    {
        GameObject bossAttack = Instantiate(bossAttackPrefab, _rb.position, quaternion.identity);

        float shift;
        if (direction)
        {
            shift = 5f;
        }
        else
        {
            shift = -5f;
        }
        bossAttack.transform.position = new Vector2(_rb.position.x + shift, _rb.position.y + 4f);
        bossAttack.transform.localScale = new Vector3(5f, 4f, 1f);

        StartCoroutine(DestroyObject(1f, bossAttack));
    }
    
    public void Smash()
    {
        GameObject bossAttack = Instantiate(bossAttackPrefab, _rb.position, quaternion.identity);

        bossAttack.transform.position = new Vector2(_rb.position.x + 3.3f, _rb.position.y);
        bossAttack.transform.localScale = new Vector3(4.6f, 2f, 1f);

        StartCoroutine(DestroyObject(0.3f, bossAttack));
        
        GameObject bossAttack2 = Instantiate(bossAttackPrefab, _rb.position, quaternion.identity);

        bossAttack2.transform.position = new Vector2(_rb.position.x - 3.3f, _rb.position.y);
        bossAttack2.transform.localScale = new Vector3(4.6f, 2f, 1f);
        
        StartCoroutine(DestroyObject(0.3f, bossAttack2));
    }
    
    private IEnumerator DestroyObject(float delay, GameObject gameObject)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
    
    public void Death()
    {
        Destroy(gameObject);
        bossLiveBar.color = new Color(1f, 0f, 0f, 0f);
        bossLive.color = new Color(1f, 0f, 0f, 0f);
        Instantiate(catPrefab, new Vector2(_rb.position.x, _rb.position.y + 1f), quaternion.identity);
    }
    
    
}
