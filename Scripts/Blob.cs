using Unity.Mathematics;
using UnityEngine;

public class Blob : MonoBehaviour
{
    private Rigidbody2D _rb;
    
    [SerializeField] private GameObject demonPrefab; 

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Death()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
        Instantiate(demonPrefab, _rb.position, quaternion.identity);
    }

}
