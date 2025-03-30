using TMPro;
using UnityEngine;

public class BotCollider : MonoBehaviour
{
    [SerializeField] private int hp = 5;
    [SerializeField] private int botScore = 1;

    private Animator _an;
    private bool _isDead;
    private Rigidbody2D _rb;

    private void Start()
    {
        _an = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Weapon"))
        {
            hp--;
        }

        if (_isDead) return;
        
        if (hp <= 0)
        {
            _an.SetTrigger("death");
            _rb.linearVelocity = Vector2.zero;
            Store.Score += botScore;
            _isDead = true;
        }
    }
    
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}