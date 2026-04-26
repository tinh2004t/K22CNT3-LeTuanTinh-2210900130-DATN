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

    // Biến lưu trữ thời gian hồi chiêu (không hiển thị ở Inspector)
    private float aoeTimer = 0f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();

        agent.speed = chaseSpeed;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(player.position);
        animator.transform.LookAt(player);

        // Trừ dần thời gian hồi chiêu của đòn AoE theo thời gian thực
        if (aoeTimer > 0)
        {
            aoeTimer -= Time.deltaTime;
        }

        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

        // -- Check if the agent should stop chasing -- //
        if (distanceFromPlayer > stopChasingDistance)
        {
            animator.SetBool("isChasing", false);
        }

        // -- Check if the agent should attack -- //
        if (distanceFromPlayer < attackingDistance)
        {
            // Kiểm tra: Nếu đòn AoE đã hồi xong (<= 0) VÀ xoay ru-lét trúng tỷ lệ %
            if (aoeTimer <= 0f && Random.Range(0f, 100f) <= aoeChance)
            {
                animator.SetBool("isAoEAttacking", true);
                aoeTimer = aoeCooldown; // Đặt lại bộ đếm thời gian hồi chiêu
            }
            else
            {
                animator.SetBool("isAttacking", true); // Nếu trượt thì đánh thường
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position); // Dừng lại khi chuyển sang trạng thái đánh
    }
}