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

    IEnumerator AttackCoroutine()
    {
        patrolAI.Pause(2.5f);
        isAttacking = true;

        PlayAnimation("SoldierFlying_Attack1");
        yield return new WaitForSeconds(.3f);
        
        stoneableBehavior.canBeStoned = true;
        yield return new WaitForSeconds(.7f);

        PlayAnimation("SoldierFlying_Attack2");
        stoneableBehavior.canBeStoned = false;
        var arrow = Instantiate(arrowPrefab, lineOfSight.position, Quaternion.identity);
        arrow.transform.localScale = new Vector3(transform.lossyScale.x, 1, 1);
        yield return new WaitForSeconds(.4f);
        
        isAttacking = false;
    }
}
