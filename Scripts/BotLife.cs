using UnityEngine;

public class BotCollider : MonoBehaviour
{
    [SerializeField] private int _hp = 5;
    
    private Animator _an;
    
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
        if (_hp <= 0)
        {
            _an.SetTrigger("death");
        }
    }
    
    public void DestroyObject()
    {
        Destroy(gameObject);
    }

}
