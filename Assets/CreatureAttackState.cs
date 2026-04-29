<<<<<<< HEAD
﻿using System;
=======
using System;
>>>>>>> origin/main
using UnityEngine;
using UnityEngine.AI;

public class CreatureAttackState : StateMachineBehaviour
{
    Transform player;
    NavMeshAgent agent;

<<<<<<< HEAD
    [Header("Attack Settings")]
    public float stopAttackingDistance = 2.5f;
    public int damageToInflict = 10;

    [Tooltip("Thời điểm gây sát thương (0.0 đến 1.0). Ví dụ: 0.5 là ngay giữa Animation")]
    public float damageTime = 0.5f;

    private bool hasDealtDamage;

=======
    public float stopAttackingDistance = 2.5f;

    public float attackRate = 1f;
    private float attackTimer;
    public int damageToInflict = 1;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
>>>>>>> origin/main
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
<<<<<<< HEAD

        hasDealtDamage = false;
    }

=======
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
>>>>>>> origin/main
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        LookAtPlayer();

<<<<<<< HEAD
        float currentAnimTime = stateInfo.normalizedTime % 1.0f;

        if (currentAnimTime >= damageTime && !hasDealtDamage)
        {
            Attack();
            hasDealtDamage = true; 
        }
        else if (currentAnimTime < damageTime && hasDealtDamage)
        {
            hasDealtDamage = false;
        }

=======
        if (attackTimer <= 0)
        {
            Attack();
            attackTimer = 1f / attackRate;
        }
        else
        {
                        attackTimer -= Time.deltaTime;
        }

        // -- Check if agent should stop attacking -- //
>>>>>>> origin/main

        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer > stopAttackingDistance)
        {
            animator.SetBool("isAttacking", false);
        }
    }

<<<<<<< HEAD
    private void LookAtPlayer()
    {
        Vector3 direction = player.position - agent.transform.position;
        if (direction != Vector3.zero)
        {
            agent.transform.rotation = Quaternion.LookRotation(direction);
            var yRotation = agent.transform.eulerAngles.y;
            agent.transform.rotation = Quaternion.Euler(0, yRotation, 0);
        }
=======
    

    private void LookAtPlayer()
    {
        Vector3 direction = player.position - agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = agent.transform.eulerAngles.y;
        agent.transform.rotation = Quaternion.Euler(0, yRotation, 0);
>>>>>>> origin/main
    }

    private void Attack()
    {
        agent.gameObject.GetComponent<Animal>().PlayAttackSound();
<<<<<<< HEAD
        PlayerState.Instance.TakeDamage(damageToInflict);
        Debug.Log("Boss chém thường! Gây " + damageToInflict + " sát thương.");
    }
}
=======

        PlayerState.Instance.TakeDamage(damageToInflict);
    }
}
>>>>>>> origin/main
