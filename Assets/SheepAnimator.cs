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
}
