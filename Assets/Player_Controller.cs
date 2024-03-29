using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Player_Controller : MonoBehaviour
{
    [SerializeField] GameObject stonePrefab;
    [SerializeField] LayerMask maskGround;
    [SerializeField] Animator animator;

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
    bool isPushing;
    bool hasWon;
    bool isDead;
    bool lockedMovementUntilGrounded;
    public bool isGrounded;
    bool canAttack = true;
    bool canMove = true;
    bool controlsLocked = false;
    Rigidbody2D rb;
    BoxCollider2D bc;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        isGrounded = true;
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !controlsLocked)
        {
            lockedMovementUntilGrounded = false;
            directionX = ctx.ReadValue<Vector2>().x;
        }
        else if (ctx.canceled)
        {
            directionX = 0;
        }
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !controlsLocked && rb.velocity.y <= 0)
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

    public void Pause(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Game_Manager.i.TogglePause();
        }
    }

    IEnumerator AttackCoroutine()
    {
        canAttack = false;
        isAttacking = true;
        LockMovement();
        PlayAnimation("Player_Attack");
        Camera_Manager.i.Shake();
        Audio_Manager.i.PlaySound("ray_shoot");
        Camera_Manager.i.RayCameraEffect(1, .1f);
        ray.RayShootStart();
        yield return new WaitForSeconds(.7f);
        ray.RayShootStop();
        Camera_Manager.i.RayCameraEffect(0, .25f);
        yield return new WaitForSeconds(.3f);
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

    public void LockMovementUntilGrounded()
    {
        Audio_Manager.i.PlaySound("player_bounce");
        lockedMovementUntilGrounded = true;
        rb.velocity = Vector2.zero;
    }

    private void Update()
    {
        AnimatorUpdate();
    }
    
    private void AnimatorUpdate()
    {
        if (isDead)
        {
            PlayAnimation("Player_Death");
        }
        else if (hasWon)
        {
            PlayAnimation("Player_Win");
        }
        else if (isAttacking)
        {
            //PlayAnimation("Player_Attack");
        } else if (isPushing)
        {
            PlayAnimation("Player_Push");
        } else if (!isGrounded)
        {
            if (rb.velocity.y > 0)
            {
                PlayAnimation("Player_Jump");
            }
            else
            {
                PlayAnimation("Player_Fall");
            }
        } else if (isGrounded)
        {
            if (directionX != 0)
            {
                PlayAnimation("Player_Walk");
            }
            else
            {
                PlayAnimation("Player_Idle");
            }
        }
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

        isGrounded = IsGrounded();
        if (isGrounded)
        {
            lockedMovementUntilGrounded = false;

            if (coyoteTimeTimer != coyoteTime)
            {
                coyoteTimeTimer = coyoteTime;
                ps_dust.Play();
                Audio_Manager.i.PlaySound("player_fall");                
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
        if (lockedMovementUntilGrounded) return;

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
        if (lockedMovementUntilGrounded) return;

        // JUMP
        if (canMove)
        {
            if (jumpPressed && coyoteTimeTimer > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
                Audio_Manager.i.PlaySound("player_jump");
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
        return Physics2D.BoxCast(bc.bounds.center - new Vector3(0, bc.bounds.size.y / 2),  new Vector2(bc.bounds.size.x - .225f, .05f), 0f, Vector2.down, 1f, maskGround);
    }

    void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawCube(bc.bounds.center - new Vector3(0, bc.bounds.size.y / 2), new Vector2(bc.bounds.size.x - .1f, .05f));
        }
    }

    void PlayAnimation(string animName)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(animName))
        {
            animator.Play(animName);
        }
    }

    public void SetIsPushing(bool isPushing)
    {
        this.isPushing = isPushing;
    }

    public void SetHasWon(bool hasWon)
    {
        this.hasWon = hasWon;
    }

    Tween movementTween;
    public void DOMovePlayer(float addedPosition, float duration = .4f)
    {
        if (movementTween == null)
        {
            movementTween = transform.DOMoveX(addedPosition, duration).SetRelative(true).OnComplete(() => movementTween = null);
        }
    }

    public void DOStopMovePlayer()
    {
        movementTween.Kill();
        movementTween = null;
    }

    public void Die()
    {
        isDead = true;
    }

    public void Stone()
    {
        Audio_Manager.i.PlaySound("ray_stone");
        gameObject.SetActive(false);
        Stoneable_Behavior stone = Instantiate(stonePrefab, transform.position, Quaternion.identity).GetComponent<Stoneable_Behavior>();
        stone.transform.localScale = transform.localScale;
        stone.StonedAnimation();
    }

    public void ToggleUIControls(bool isUI)
    {
        if (isUI)
        {
            GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
        } else
        {
            GetComponent<PlayerInput>().SwitchCurrentActionMap("GamePlay");
        }
    }
}
