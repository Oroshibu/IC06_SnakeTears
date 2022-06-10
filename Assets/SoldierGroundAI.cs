using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierGroundAI : MonoBehaviour
{
    [SerializeField] Stoneable_Behavior stoneableBehavior;
    [SerializeField] TriggerComponent triggerSword;
    [SerializeField] TriggerComponent triggerDamage;

    Animator animator;
    PatrolAI patrolAI;
    
    bool isAttacking;

    // Start is called before the first frame update
    void Start()
    {   
        animator = GetComponent<Animator>();
        patrolAI = GetComponent<PatrolAI>();

        triggerSword.onTrigger += OnSwordTriggered;
        triggerDamage.onTrigger += OnDamageTriggered;        
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking)
        {
            //PlayAnimation("SoldierGround_Attack1");
        }
        else if (patrolAI.isMoving)
        {
            PlayAnimation("SoldierGround_Walk");
        }
        else
        {
            PlayAnimation("SoldierGround_Idle");
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

    void PlayAnimation(string animName)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(animName))
        {
            animator.Play(animName);
        }
    }

    private void OnDamageTriggered(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Game_Manager.i.Death();
        }
    }

    private void OnSwordTriggered(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isAttacking)
        {
            StartCoroutine(AttackCoroutine());   
        }
    }

    IEnumerator AttackCoroutine()
    {
        patrolAI.Pause(2.5f);
        isAttacking = true;

        Audio_Manager.i.PlaySound("soldierg_attack1");
        PlayAnimation("SoldierGround_Attack1");
        yield return new WaitForSeconds(1f);


        triggerDamage.gameObject.SetActive(true);
        triggerDamage.GetComponent<BoxCollider2D>().isTrigger = false;
        triggerDamage.GetComponent<BoxCollider2D>().isTrigger = true;

        Audio_Manager.i.PlaySound("soldierg_attack2");
        PlayAnimation("SoldierGround_Attack2");
        yield return new WaitForSeconds(.25f);

        triggerDamage.gameObject.SetActive(false);
        stoneableBehavior.canBeStoned = true;
        yield return new WaitForSeconds(1.25f);
        
        
        stoneableBehavior.canBeStoned = false;
        isAttacking = false;
    }
}
