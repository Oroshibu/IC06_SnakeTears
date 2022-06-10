using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierFlyingAI : MonoBehaviour
{
    [SerializeField] Stoneable_Behavior stoneableBehavior;
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] LayerMask sightMask;
    [SerializeField] Transform lineOfSight;

    Animator animator;
    PatrolAI patrolAI;

    bool isAttacking;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        patrolAI = GetComponent<PatrolAI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking)
        {
            //PlayAnimation("SoldierFlying_Attack1");
        }
        else
        {
            PlayAnimation("SoldierFlying_Idle");
        }
    }

    private void FixedUpdate()
    {
        RaycastHit2D hitSight = Physics2D.Raycast(lineOfSight.position, Vector2.right * transform.lossyScale.x, 30f, sightMask);

        if (hitSight.collider != null)
        {
            Debug.Log("Hit Sight " + hitSight.collider.name);

            if (hitSight.collider.CompareTag("Player"))
            {
                if (!isAttacking)
                {
                    StartCoroutine(AttackCoroutine());
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Game_Manager.i.Death();            

            PushPlayerBack(collision);
            if (!isAttacking)
            {
                FlipTowards(collision.transform.position);
                StartCoroutine(AttackCoroutine());
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

    private void FlipTowards(Vector3 position)
    {
        if (transform.position.x - position.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    void PlayAnimation(string animName)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(animName))
        {
            animator.Play(animName);
        }
    }

    IEnumerator AttackCoroutine()
    {
        patrolAI.Pause(2.5f);
        isAttacking = true;

        Audio_Manager.i.PlaySound("soldierf_attack1");
        PlayAnimation("SoldierFlying_Attack1");
        yield return new WaitForSeconds(.3f);
        
        stoneableBehavior.canBeStoned = true;
        yield return new WaitForSeconds(.7f);

        Audio_Manager.i.PlaySound("soldierf_attack2");        
        PlayAnimation("SoldierFlying_Attack2");
        stoneableBehavior.canBeStoned = false;
        var arrow = Instantiate(arrowPrefab, lineOfSight.position, Quaternion.identity);
        arrow.transform.localScale = new Vector3(transform.lossyScale.x, 1, 1);
        yield return new WaitForSeconds(.4f);
        
        isAttacking = false;
    }
}
