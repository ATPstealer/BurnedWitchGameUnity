using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{

    private Animator _an;
    private Rigidbody2D _rb;
    private BoxCollider2D _bc;
    
    void Start()
    {
        Store.dead = false;
        
        _an = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _bc = GetComponent<BoxCollider2D>();
        
        _an.ResetTrigger("death");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DeadlyBody"))
        {
            if (!Store.dead)
            {
                _an.SetTrigger("death");
                // Hack position because of death animation is very low
                _bc.offset = new Vector2(_bc.offset.x, _bc.offset.y - 1.3f); 
                _rb.position = new Vector2(_rb.position.x, _rb.position.y + 1.33f);
                
                Store.dead = true;
                Store.Lives--;
                if (Store.Lives <= 0)
                {
                    SceneManager.LoadScene(0);
                }
            }
        }
    }
}
