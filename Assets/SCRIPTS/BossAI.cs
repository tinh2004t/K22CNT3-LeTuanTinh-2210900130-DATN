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

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

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
        agent.isStopped = false; 
        agent.SetDestination(player.position); 

    }

    void AttackState()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            agent.isStopped = true; 

            Debug.Log("Boss đã vào tầm đánh và đang Tấn công!");
        }

        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}