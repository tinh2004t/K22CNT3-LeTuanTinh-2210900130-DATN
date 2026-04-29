<<<<<<< HEAD
﻿using UnityEngine;
=======
using UnityEngine;
>>>>>>> origin/main
using UnityEngine.AI;

public class CreatureChaseState : StateMachineBehaviour
{
    NavMeshAgent agent;
    Transform player;

<<<<<<< HEAD
    [Header("Chase Settings")]
    public float chaseSpeed = 6f;
    public float stopChasingDistance = 21f;
    public float attackingDistance = 2.5f;

    [Header("AoE Attack Logic")]
    [Tooltip("Tỷ lệ phần trăm tung đòn AoE (0 - 100)")]
    public float aoeChance = 30f;
    [Tooltip("Thời gian hồi chiêu của đòn AoE (giây)")]
    public float aoeCooldown = 8f;

    private float aoeTimer = 0f;

=======
    public float chaseSpeed = 6f;

    public float stopChasingDistance = 21;
    public float attackingDistance = 2.5f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
>>>>>>> origin/main
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();

        agent.speed = chaseSpeed;
    }

<<<<<<< HEAD
=======
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
>>>>>>> origin/main
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(player.position);
        animator.transform.LookAt(player);

<<<<<<< HEAD
        if (aoeTimer > 0)
        {
            aoeTimer -= Time.deltaTime;
        }

        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

=======
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

        // -- Check if the agent should stop chasing -- //
>>>>>>> origin/main
        if (distanceFromPlayer > stopChasingDistance)
        {
            animator.SetBool("isChasing", false);
        }

<<<<<<< HEAD
        if (distanceFromPlayer < attackingDistance)
        {
            if (aoeTimer <= 0f && Random.Range(0f, 100f) <= aoeChance)
            {
                animator.SetBool("isAoEAttacking", true);
                aoeTimer = aoeCooldown; 
            }
            else
            {
                animator.SetBool("isAttacking", true); 
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position); 
    }
}
=======
        // -- Check if the agent should attack -- //
        if (distanceFromPlayer < attackingDistance)
        {
            animator.SetBool("isAttacking", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
    }
}
>>>>>>> origin/main
