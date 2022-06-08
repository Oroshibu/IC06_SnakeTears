using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepAnimator : MonoBehaviour
{
    Animator animator;
    PatrolAI patrolAI;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        patrolAI = GetComponent<PatrolAI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (patrolAI.isMoving)
        {
            PlayAnimation("Sheep_Walk");
        } else
        {
            PlayAnimation("Sheep_Idle");
        }
    }

    void PlayAnimation(string animName)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(animName))
        {
            animator.Play(animName);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //push player back
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.transform.position.y -transform.position.y > 0.5f)
            {
                PushPlayerBack(collision);
                patrolAI.Pause();
            }

        }
    }

    private void PushPlayerBack(Collision2D collision)
    {
        if (collision.transform.position.y - transform.position.y > 0.5f)
        {
            collision.gameObject.GetComponent<Player_Controller>().LockMovementUntilGrounded();
            if (transform.position.x > collision.transform.position.x)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-300, 500), ForceMode2D.Impulse);
            }
            else
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(300, 500), ForceMode2D.Impulse);
            }
        }
    }
}
