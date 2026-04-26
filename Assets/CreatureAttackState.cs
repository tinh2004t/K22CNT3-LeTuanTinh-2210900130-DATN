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

    // Biến cờ đánh dấu xem nhịp này đã trừ máu chưa
    private bool hasDealtDamage;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();

        // Reset cờ sát thương khi mới bắt đầu vào trạng thái đánh
        hasDealtDamage = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        LookAtPlayer();

        // -- Xử lý đồng bộ sát thương với Animation -- //

        // Lấy thời gian chạy hiện tại của animation (từ 0.0 đến 1.0). 
        // Dùng % 1.0f để hỗ trợ trường hợp Animation được set lặp lại (Loop Time)
        float currentAnimTime = stateInfo.normalizedTime % 1.0f;

        // Nếu animation chạy đến thời điểm vung kiếm (damageTime) và chưa trừ máu
        if (currentAnimTime >= damageTime && !hasDealtDamage)
        {
            Attack();
            hasDealtDamage = true; // Đánh dấu là đã trừ máu rồi, không trừ liên tục nữa
        }
        // Khi animation lặp lại vòng mới (currentAnimTime quay về 0) thì reset lại cờ
        else if (currentAnimTime < damageTime && hasDealtDamage)
        {
            hasDealtDamage = false;
        }

        // -- Check if agent should stop attacking -- //

        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer > stopAttackingDistance)
        {
            animator.SetBool("isAttacking", false);
        }
    }

    private void LookAtPlayer()
    {
        Vector3 direction = player.position - agent.transform.position;
        // Tránh lỗi khi khoảng cách quá gần bằng 0
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