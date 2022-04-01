using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controller : MonoBehaviour
{

    [SerializeField] LayerMask maskJumpableGround;

    public float speed = 1f;
    public float jumpVelocity = 1f;
    public float gravityIdle = 1f;
    public float gravityFall = 1f;
    public float coyoteTime = .5f;
    //public float lowFallMultiplier = 1f;

    float directionX;
    public float coyoteTimeTimer;
    bool jumpPressed;
    bool jumpHeld;
    Rigidbody2D rb;
    BoxCollider2D bc;
    Ray_Controller ray;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        ray = GetComponentInChildren<Ray_Controller>();
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            jumpPressed = true;
            jumpHeld = true;
        }
        else if (ctx.canceled)
        {
            jumpHeld = false;
        }

    }

    public void Attack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {

        }
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            directionX = ctx.ReadValue<Vector2>().x;
        }
        else if (ctx.canceled)
        {
            directionX = 0;
        }
    }

    private void FixedUpdate()
    {
        //FLIP PLAYER
        if (directionX < 0)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
        else if (directionX > 0)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }

        if (IsGrounded())
        {
            coyoteTimeTimer = coyoteTime;
        } else
        {
            coyoteTimeTimer -= Time.fixedDeltaTime;
        }

        FixedUpdateWalk();

        FixedUpdateJump();
    }

    void FixedUpdateWalk()
    {
        // WALK
        rb.velocity = new Vector2(directionX * speed, rb.velocity.y);
    }

    void FixedUpdateJump()
    {
        // JUMP
        if (jumpPressed && coyoteTimeTimer > 0)
        {
            rb.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
            jumpPressed = false;
        }

        //SMOOTHIFICATION DU SAUT
        if (rb.velocity.y < .000001f) // si on est en chute
        {
            rb.gravityScale = gravityFall;
        }
        else // si on est au repos
        {
            rb.gravityScale = gravityIdle;
        }

        //else if (rb.velocity.y > 0.0001 && (!jump || isAttacking)) // si on est en ascension et qu'on appuie plus sur saut
        if (rb.velocity.y > .000001f && !jumpHeld)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            //rb.gravityScale = lowFallMultiplier;
        }

    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(bc.bounds.center, bc.bounds.size * new Vector2(.75f, 1), 0f, Vector2.down, .2f, maskJumpableGround);
    }
}
