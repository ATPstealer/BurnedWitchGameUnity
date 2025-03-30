using TMPro;
using UnityEngine;

public class BotCollider : MonoBehaviour
{
    [SerializeField] private int _hp = 5;

    private Animator _an;
    private bool _isDead;

    private void Start()
    {
        _an = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Weapon"))
        {
            _hp--;
        }

        if (_isDead) return;
        
        if (_hp <= 0)
        {
            _an.SetTrigger("death");
            Store.Score += 1;
            Debug.Log("ScoreChange");
            _isDead = true;
        }
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}