using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controller : MonoBehaviour
{
    
    public float speed = 1f;
    public float jumpVelocity = 1f;
    public float gravityIdle = 1f;
    public float gravityFall = 1f;
    //public float lowFallMultiplier = 1f;

    float directionX;
    bool grounded;
    bool jumpPressed;
    bool jumpHeld;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        if (jumpPressed)
        {
            rb.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
            jumpPressed = false;
            grounded = false;
        }
        else
        {
            //Vector2 boxCenter = (Vector2)transform.position + Vector2.down * (playerSize.y + boxSize.y) * 0.5f + colliderOffset;
            ////bool atterissage = grounded;
            //grounded = Physics2D.OverlapBox(boxCenter, boxSize, 0f, mask) != null; // est ce qu'on détecte qlq chose sous le j
            //if (grounded)
            //{
            //    coyoteJumpTimer = coyoteJumpTime;
            //    if (atterissage == false) // On vient d'atterrir au sol
            //    {
            //        //cameraAnimator.SetTrigger("Shake");
            //    }
            //}
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
}
