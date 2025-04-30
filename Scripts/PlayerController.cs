using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float flySpeed = 50f;
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private GameObject fireballPrefab; 
    [SerializeField] private GameObject thunderPrefab; 
    [SerializeField] private GameObject attackBoxPrefab; 
    
    private PlayerInput _pi;
    private Rigidbody2D _rb;
    private Animator _an;
    private SpriteRenderer _sr;
    private BoxCollider2D _bc;
    
    private enum State { Idle, Run, Jump, Fall, Fire, Attack, Fly, Thunder }
    private bool _fly = false;
    private bool _flyBlock = false;
    private bool _fire = false;
    private bool _thunder = false;
    private bool _attack = false;
    private bool _direction = true; // Right is true
    private const float FireballCost = 10f;
    private const float ThunderCost = 50f;
    
    
    void Start()
    {
        _pi = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody2D>();
        _an = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
        _bc = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (Store.dead)
        {
            LevelRestart();
            return;
        };

        MovementHandle();
        FlyHandle();
        CastHandle();
        AttackHandle();
        AnimationChoose();
        ManaRegen();
    }

    private void MovementHandle()
    {
        var jx = _pi.actions["Move"].ReadValue<Vector2>().x;
        var cv = _rb.linearVelocity;
        
        // Run or Idle
        if (math.abs(jx) > 0.1f)
        {
            cv.x = jx * speed;
        }
        else
        {
            cv.x = 0;
        }
        
        // Jump 
        if (_pi.actions["Jump"].WasPressedThisFrame() && IsGrounded())
        {
            if (Store.Mana > 5f)
            {
                cv.y += jumpForce;
                // Cost Jump
                Store.Mana -= 5f;
            } 
        }
        
        // Restrict fast speed
        if (cv.x > Store.maxSpeed) cv.x = Store.maxSpeed;
        if (cv.y > Store.maxSpeed) cv.y = Store.maxSpeed;
        
        // Return velocity value and Choose animation according speed
        _rb.linearVelocity = cv;
    }

    private void FlyHandle()
    {
        var cv = _rb.linearVelocity;
        if (_pi.actions["Fly"].IsPressed() && !IsGrounded())
        {
            if (Store.Mana > 1f && !_flyBlock)
            {
                Store.Mana -= 30f * Time.deltaTime;
                _fly = true;
                if (cv.y < 0)
                {
                    cv.y += flySpeed * Time.deltaTime;
                    _rb.linearVelocity = cv;
                }
            }
            else
            {
                _flyBlock = true;
                _fly = false;
            }
        }
        else
        {
            _fly = false;
        }

        if (_flyBlock && IsGrounded())
        {
            _flyBlock = false;
        }
        
    }

    private void AnimationChoose()
    {
        State state = State.Idle;
        var v = _rb.linearVelocity;
        
        // Set direction for blasts
        if (v.x > 0.1f)
        {
            _direction = true;
        }
        if (v.x < -0.1f)
        {
            _direction = false;
        }
        
        // Mirror sprites if player go left.
        _sr.flipX = !_direction;
        
        state = math.abs(v.x) > 0.1f ? State.Run : State.Idle;

        if (v.y > 2)
        {
            state = State.Jump;
        }
        
        if (v.y < -2)
        {
            state = State.Fall;
        }

        if (_fly)
        {
            state = State.Fly;
        }

        if (_fire)
        {
            state = State.Fire;
        }
        
        if (_attack)
        {
            state = State.Attack;
        }
        
        if (_thunder)
        {
            state = State.Thunder;
        }

        _an.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(_bc.bounds.center, _bc.bounds.size, 0f, Vector2.down, 0.1f, 1 << LayerMask.NameToLayer("Ground"));
    }

    private void LevelRestart()
    {
        if (_pi.actions["Jump"].WasPressedThisFrame())
        {
            Store.MessageUI = "";
            SceneManager.LoadScene(Store.level);
        }
    }

    private void CastHandle()
    {
        if (_pi.actions["Fire"].WasPressedThisFrame() && Store.Mana > FireballCost)
        {
            _fire = true;
            ShootFireball(); 
        }
        else
        {
            _fire = false;
        }
        
        if (_pi.actions["Thunder"].WasPressedThisFrame() && Store.Mana > ThunderCost)
        {
            _thunder = true;
            ShootThunder(); 
        }
        else
        {
            _thunder = false;
        }
    }
    
    private void ShootFireball()
    {
        // Cost Fireball
        // Mana withdraw
        if (Store.Mana < FireballCost) return;
        Store.Mana -= FireballCost;
        
        // Fireball position
        float xShift = _direction ? .9f : -.9f;
        Vector2 fireballPosition = new Vector2(_rb.position.x + xShift, _rb.position.y);
        
        // Create fireball and speed
        GameObject fireball = Instantiate(fireballPrefab, fireballPosition, quaternion.identity);
        Rigidbody2D fireballRb = fireball.GetComponent<Rigidbody2D>();
        float fireballSpeed = _direction ? 7f : -7f;
        fireballRb.linearVelocity = new Vector2(fireballSpeed, 0);
        SpriteRenderer fireballSr = fireball.GetComponent<SpriteRenderer>();
        fireballSr.flipX = !_direction;
    }
    
    private void ShootThunder()
    {
        // Cost Thunderstorm
        // Mana withdraw
        if (Store.Mana < ThunderCost) return;
        Store.Mana -= ThunderCost;
        
        // Create 3 thunderstorms
        StartCoroutine(CreateThunderstorm(0.2f));
        StartCoroutine(CreateThunderstorm(0.3f));
        StartCoroutine(CreateThunderstorm(0.4f));
    }

    IEnumerator CreateThunderstorm(float delay)
    {
        yield return new WaitForSeconds(delay);
        // Thunder position
        float xShift = _direction ? 1f : -1f;
        Vector2 thunderPosition = new Vector2(_rb.position.x + xShift, _rb.position.y);
        
        // Creating
        GameObject thunder = Instantiate(thunderPrefab, thunderPosition, quaternion.identity);
        Rigidbody2D thunderRb = thunder.GetComponent<Rigidbody2D>();
        float thunderSpeed = _direction ? 22f : -22f;
        thunderRb.linearVelocity = new Vector2(thunderSpeed, 0);
        SpriteRenderer thunderSr = thunder.GetComponent<SpriteRenderer>();
        thunderSr.flipX = !_direction;
    }
    
    private void AttackHandle()
    {
        if (_currentAttackBox != null)
        {
            Attack();
            return;
        }
        
        if (_pi.actions["Attack"].WasPressedThisFrame())
        {
            _attack = true;
            Attack(); 
        }
        else
        {
            _attack = false;
        }
    }

    private GameObject _currentAttackBox;
    private Rigidbody2D _currentAttackBoxTransform;
    
    private void Attack()
    {
        // Attack Box position
        float xShift = _direction ? 1.4f : -1.4f;
        Vector2 attackBoxPosition = new Vector2(_rb.position.x + xShift, _rb.position.y);
        
        if (_currentAttackBox != null)
        {
            _currentAttackBoxTransform.MovePosition(new Vector2(_rb.position.x + xShift, _rb.position.y));
            return; // Exit if an AttackBox already exists
        }
        
        // Create fireball and speed
        _currentAttackBox = Instantiate(attackBoxPrefab, attackBoxPosition, quaternion.identity);
        _currentAttackBoxTransform = _currentAttackBox.GetComponent<Rigidbody2D>();
        
        // Destroy Attack Box after delay
        StartCoroutine(DestroyAttackBox());
    }
    
    private IEnumerator DestroyAttackBox()
    {
        yield return new WaitForSeconds(1.3f);
        Destroy(_currentAttackBox);
        _currentAttackBox = null;
        _currentAttackBoxTransform = null;
    }
    
    private void ManaRegen()
    {
        if (Store.Mana < Store.manaMax)
        {
            Store.Mana += Store.manaRegen * Time.deltaTime;
        }
    }

}