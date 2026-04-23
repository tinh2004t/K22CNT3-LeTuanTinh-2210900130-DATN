using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;

    [Header("Cấu hình AI")]
    public float chaseSpeed = 5f;
    public float stoppingDistance = 2.5f;

    // Biến kiểm soát trạng thái
    private bool isAttacking = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = chaseSpeed;
        agent.stoppingDistance = stoppingDistance;

        // Tự động tìm Player nhờ Tag đã gắn ở Phần 1
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        // Tính khoảng cách thực tế giữa Boss và Player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Logic Máy trạng thái đơn giản
        if (distanceToPlayer <= agent.stoppingDistance)
        {
            AttackState();
        }
        else
        {
            ChaseState();
        }
    }

    void ChaseState()
    {
        isAttacking = false;
        agent.isStopped = false; // Cho phép NavMeshAgent di chuyển
        agent.SetDestination(player.position); // Đặt mục tiêu là vị trí của Player

        // TODO: Đặt code chạy Animation "Đi/Chạy" ở đây
    }

    void AttackState()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            agent.isStopped = true; // Dừng di chuyển khi đang ra đòn

            Debug.Log("Boss đã vào tầm đánh và đang Tấn công!");
            // TODO: Đặt code chạy Animation "Đánh thường" ở đây
        }

        // Từ từ xoay mặt Boss về phía Player kể cả khi đang đứng yên đánh
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}