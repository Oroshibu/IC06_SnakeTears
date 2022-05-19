using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controller : MonoBehaviour
{

    [SerializeField] LayerMask maskGround;

    [Header("Movement")]
    public float speed = 1f;
    public float jumpVelocity = 1f;
    public float gravityIdle = 1f;
    public float gravityFall = 1f;
    public float coyoteTime = .5f;

    [Header("Attack")]
    public float attackCooldown = .25f;

    [Header("References")]
    public Ray_Controller ray;


    public ParticleSystem ps_dust;

    float directionX;
    public Vector2 direction { get => new Vector2(directionX, 0); }
    float coyoteTimeTimer;
    bool jumpPressed;
    bool jumpHeld;
    bool isAttacking;
    bool canAttack = true;
    bool canMove = true;
    bool controlsLocked = false;
    Rigidbody2D rb;
    BoxCollider2D bc;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !controlsLocked)
        {
            directionX = ctx.ReadValue<Vector2>().x;
        }
        else if (ctx.canceled)
        {
            directionX = 0;
        }
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !controlsLocked)
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
        if (ctx.performed && !controlsLocked)
        {
            if (!isAttacking && canAttack)
            {
                StartCoroutine(AttackCoroutine());
            }

        }
    }

    public void Restart(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !controlsLocked)
        {
            Game_Manager.i.Restart();
        }
    }

    IEnumerator AttackCoroutine()
    {
        canAttack = false;
        isAttacking = true;
        LockMovement();
        Camera_Manager.i.Shake();
        Camera_Manager.i.RayCameraEffect(1, .1f);
        ray.RayShootStart();
        yield return new WaitForSeconds(1);
        ray.RayShootStop();
        Camera_Manager.i.RayCameraEffect(0, .25f);
        UnlockMovement();
        isAttacking = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    public void LockMovement()
    {
        canMove = false;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
    }

    public void UnlockMovement()
    {
        canMove = true;
        rb.isKinematic = false;
    }

    public void LockControls()
    {
        controlsLocked = true;
        canMove = false;
        rb.velocity = Vector2.zero;
    }

    public void UnlockControls()
    {
        canMove = true;
        controlsLocked = false;
    }

    private void FixedUpdate()
    {
        //FLIP PLAYER
        if (canMove)
        {
            if (directionX < 0)
            {
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            }
            else if (directionX > 0)
            {
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            }
        }


        if (IsGrounded())
        {
            if (coyoteTimeTimer != coyoteTime)
            {
                coyoteTimeTimer = coyoteTime;
                ps_dust.Play();
            }

            transform.position = new Vector2(transform.position.x, Mathf.RoundToInt(transform.position.y * 25) / 25f);
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
        if (canMove)
        {
            rb.velocity = new Vector2(directionX * speed, rb.velocity.y);
        } else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    void FixedUpdateJump()
    {
        // JUMP
        if (canMove)
        {
            if (jumpPressed && coyoteTimeTimer > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
                jumpPressed = false;
                ps_dust.Play();
                coyoteTimeTimer = 0;
            }
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
        if (rb.velocity.y > .000001f && !jumpHeld && coyoteTimeTimer <= -0.15f && canMove)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }

    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(bc.bounds.center - new Vector3(0, bc.bounds.size.y / 2),  new Vector2(bc.bounds.size.x * .75f, .05f), 0f, Vector2.down, .2f, maskGround);
    }

    void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawCube(bc.bounds.center - new Vector3(0, bc.bounds.size.y / 2), new Vector2(bc.bounds.size.x * .75f, .05f));
        }
    }
}
