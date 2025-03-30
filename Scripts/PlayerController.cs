using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float flySpeed = 30f;
    [SerializeField] private float jumpForce = 20f;
    [SerializeField] private GameObject fireballPrefab; 
    [SerializeField] private GameObject attackBoxPrefab; 
    
    private PlayerInput _pi;
    private Rigidbody2D _rb;
    private Animator _an;
    private SpriteRenderer _sr;
    private BoxCollider2D _bc;
    
    private enum State { Idle, Run, Jump, Fall, Fire, Attack, Fly }
    private bool _fly = false;
    private bool _fire = false;
    private bool _attack = false;
    private bool _direction = true; // Right is true
    
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
        CastHandle();
        AttackHandle();
        AnimationChoose();
        ManaRegen();
    }

    private void MovementHandle()
    {
        // TODO: Somewhere the joystick gets caught and the movement breaks.
        var jx = _pi.actions["Move"].ReadValue<Vector2>().x;
        var jy = _pi.actions["Move"].ReadValue<Vector2>().y;
        
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
        
        // Jump or Fly
        if (math.abs(jy) > 0.7f)
        {
            if (Store.Mana > 1f)
            {
                cv.y += jy * flySpeed * Time.deltaTime;
                // Cost Fly
                Store.Mana -= 30f * Time.deltaTime;
                _fly = true;
            }
        }
        else
        {
            _fly = false;
        }
        
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
        if (_pi.actions["Fire"].WasPressedThisFrame())
        {
            _fire = true;
            ShootFireball(); 
        }
        else
        {
            _fire = false;
        }
    }
    
    private void ShootFireball()
    {
        // Cost Fireball
        // Mana withdraw
        float fireballCost = 10f;
        if (Store.Mana < fireballCost) return;
        Store.Mana -= fireballCost;
        
        // Fireball position
        float xShift = _direction ? .9f : -.9f;
        Vector2 fireballPosition = new Vector2(_rb.position.x + xShift, _rb.position.y);
        
        // Create fireball and speed
        GameObject fireball = Instantiate(fireballPrefab, fireballPosition, quaternion.identity);
        Rigidbody2D fireballRb = fireball.GetComponent<Rigidbody2D>();
        float fireballSpeed = _direction ? 10f : -10f;
        fireballRb.linearVelocity = new Vector2(fireballSpeed, 0);
        SpriteRenderer fireballSr = fireball.GetComponent<SpriteRenderer>();
        fireballSr.flipX = !_direction;
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