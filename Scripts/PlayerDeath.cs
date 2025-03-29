using System;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{

    private Animator _an;
    private Rigidbody2D _rb;
    private BoxCollider2D _bc;
    
    void Start()
    {
        _an = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _bc = GetComponent<BoxCollider2D>();
        
        _an.ResetTrigger("death");
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DeadlyBody"))
        {
            Debug.Log("Death");
            if (!Store.dead)
            {
                _an.SetTrigger("death");
                // Hack position because of death animation is very low
                Vector2 ofsset = _bc.offset;
                ofsset.y -= 1.3f;
                _bc.offset = ofsset; 
                
                Store.dead = true;
            }
        }
    }
}
