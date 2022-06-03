using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAI : MonoBehaviour
{
    public float speed = 1f;
    public float pauseTime = 1f;

    public LayerMask groundMask;
    public LayerMask wallMask;
    
    public Transform groundCheck;
    public Transform wallCheck;
    
    public bool shouldGroundCheck = true;
    public bool idiot;

    [HideInInspector] public bool isMoving = false;
    
    Rigidbody2D rb;
    float pauseTimer;
    bool pause = false;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (idiot) return;
        if (pause)
        {
            isMoving = false;
            pauseTimer -= Time.fixedDeltaTime;
            if (pauseTimer < 0)
            {
                pauseTimer = pauseTime;
                pause = false;
                Flip();
            }
        } else
        {
            isMoving = true;
            rb.MovePosition(rb.position + Vector2.right * Mathf.Sign(transform.lossyScale.x) * speed * Time.fixedDeltaTime);

            if (shouldGroundCheck)
            {
                RaycastHit2D hitGround = Physics2D.Raycast(groundCheck.position, Vector2.down, .001f, groundMask);

                if (hitGround.collider == null)
                {
                    Pause();
                }
            }

            RaycastHit2D hitWall = Physics2D.Raycast(wallCheck.position, Vector2.down, .001f, wallMask);

            if (hitWall.collider != null)
            {
                Pause();
            }
        }
    }

    public void Pause(){
        pause = true;
        pauseTimer = pauseTime;
    }

    public void Pause(float duration)
    {
        pause = true;
        pauseTimer = duration;
    }

    void Flip()
    {
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }
}
