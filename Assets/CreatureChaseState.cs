using UnityEngine;
using UnityEngine.AI;

public class CreatureChaseState : StateMachineBehaviour
{
    NavMeshAgent agent;
    Transform player;

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

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();

        agent.speed = chaseSpeed;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(player.position);
        animator.transform.LookAt(player);

        if (aoeTimer > 0)
        {
            aoeTimer -= Time.deltaTime;
        }

        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

        if (distanceFromPlayer > stopChasingDistance)
        {
            animator.SetBool("isChasing", false);
        }

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