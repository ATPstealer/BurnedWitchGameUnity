using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float flySpeed = 35f;
    [SerializeField] private float jumpForce = 20f;
    
    private PlayerInput _pi;
    private Rigidbody2D _rb;
    private Animator _an;
    private SpriteRenderer _sr;
    private BoxCollider2D _bc;
    
    private enum State { Idle, Run, Jump, Fall }
    
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
        if (Store.dead) return;
        
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
        if (math.abs(jy) > 0.1f)
        {
            cv.y += jy * flySpeed * Time.deltaTime;
        }
        
        if (_pi.actions["Jump"].WasPressedThisFrame() && IsGrounded())
        {
            cv.y += jumpForce;
        }

        
        // Return velocity value and Choose animation according speed
        _rb.linearVelocity = cv;
        AnimationChoose();
    }

   private void AnimationChoose()
    {
        State state = State.Idle;
        var v = _rb.linearVelocity;
        
        // Mirror sprites if player go left.
        if (v.x < 0.1f)
        {
            _sr.flipX = true;    
        }

        if (v.x > -0.1f)
        {
            _sr.flipX = false;
        }
        
        state = math.abs(v.x) > 0.1f ? State.Run : State.Idle;

        if (v.y > 2)
        {
            state = State.Jump;
        }
        
        if (v.y < -2)
        {
            state = State.Fall;
        }

        _an.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(_bc.bounds.center, _bc.bounds.size, 0f, Vector2.down, 0.1f, 1 << LayerMask.NameToLayer("Ground"));
    }
}
