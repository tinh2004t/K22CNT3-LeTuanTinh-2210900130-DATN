using System;
using UnityEngine;
using UnityEngine.AI;

public class CreatureAttackState : StateMachineBehaviour
{
    Transform player;
    NavMeshAgent agent;

    [Header("Attack Settings")]
    public float stopAttackingDistance = 2.5f;
    public int damageToInflict = 10;

    [Tooltip("Thời điểm gây sát thương (0.0 đến 1.0). Ví dụ: 0.5 là ngay giữa Animation")]
    public float damageTime = 0.5f;

    private bool hasDealtDamage;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();

        hasDealtDamage = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        LookAtPlayer();

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


        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer > stopAttackingDistance)
        {
            animator.SetBool("isAttacking", false);
        }
    }

    private void LookAtPlayer()
    {
        Vector3 direction = player.position - agent.transform.position;
        if (direction != Vector3.zero)
        {
            agent.transform.rotation = Quaternion.LookRotation(direction);
            var yRotation = agent.transform.eulerAngles.y;
            agent.transform.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }

    private void Attack()
    {
        agent.gameObject.GetComponent<Animal>().PlayAttackSound();
        PlayerState.Instance.TakeDamage(damageToInflict);
        Debug.Log("Boss chém thường! Gây " + damageToInflict + " sát thương.");
    }
}