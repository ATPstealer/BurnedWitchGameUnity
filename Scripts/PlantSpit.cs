using System;
using Unity.Mathematics;
using UnityEngine;

public class PlantSpit : MonoBehaviour
{
    [SerializeField] private bool _direction = false;
    [SerializeField] private GameObject bulletPrefab; 

    private Rigidbody2D _rb;
    private SpriteRenderer _sr;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (!_direction)
        {
            _sr = GetComponent<SpriteRenderer>();
            _sr.flipX = true;
        }
    }

    private void ShootBullet()
    {
        float shiftX = _direction ? -1f : 1f;
        Vector2 bulletPosition = new Vector2(_rb.position.x + shiftX, _rb.position.y);
        
        // Create fireball and speed
        GameObject bullet = Instantiate(bulletPrefab, bulletPosition, quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        float bulletSpeed = _direction ? -10f : 10f;
        bulletRb.linearVelocity = new Vector2(bulletSpeed, 0);
    }
}
