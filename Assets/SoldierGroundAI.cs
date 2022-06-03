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
            Game_Manager.i.Death();
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
        
        PlayAnimation("SoldierGround_Attack1");
        yield return new WaitForSeconds(1f);


        triggerDamage.gameObject.SetActive(true);
        triggerDamage.GetComponent<BoxCollider2D>().isTrigger = false;
        triggerDamage.GetComponent<BoxCollider2D>().isTrigger = true;
        
        PlayAnimation("SoldierGround_Attack2");
        yield return new WaitForSeconds(.25f);

        triggerDamage.gameObject.SetActive(false);
        stoneableBehavior.canBeStoned = true;
        yield return new WaitForSeconds(1.25f);
        
        
        stoneableBehavior.canBeStoned = false;
        isAttacking = false;
    }
}
