using System;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{

    private Animator _an;
    private bool _dead = false;
    
    void Start()
    {
        _an = GetComponent<Animator>();
        _an.ResetTrigger("death");
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DeadlyBody"))
        {
            Debug.Log("Death");
            if (!_dead)
            {
                _an.SetTrigger("death");
                _dead = true;
            }
        }
    }
}
