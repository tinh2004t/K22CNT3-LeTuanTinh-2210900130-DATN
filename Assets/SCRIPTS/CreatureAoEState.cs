using UnityEngine;
using UnityEngine.AI;

public class CreatureAoEState : StateMachineBehaviour
{
    [Header("Cấu hình đòn AoE")]
    public float aoeRadius = 5f;
    public int aoeDamage = 30;
    public float damageTime = 0.5f;

    [Header("Hiệu ứng & Cảnh báo")]
    public GameObject warningPrefab; // Kéo Prefab RedWarningCircle vào đây
    public GameObject aoeEffectPrefab; // Hiệu ứng nổ bùm (nếu có)

    private bool hasDealtDamage;
    private GameObject currentWarningObject; // Biến lưu trữ vòng đỏ đang hiển thị

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hasDealtDamage = false;

        NavMeshAgent agent = animator.GetComponent<NavMeshAgent>();
        if (agent != null) agent.isStopped = true;

        // --- SINH RA VÒNG CẢNH BÁO ---
        if (warningPrefab != null)
        {
            // Đặt vòng đỏ ngay dưới chân Boss, nhích lên trục Y một chút (0.1f) để không bị chìm dưới mặt đất
            Vector3 spawnPos = animator.transform.position + Vector3.up * 0.1f;
            currentWarningObject = Instantiate(warningPrefab, spawnPos, warningPrefab.transform.rotation);

            // Tự động scale vòng đỏ to bằng đúng phạm vi sát thương
            // Nhân 2 vì aoeRadius là bán kính, còn Scale trong Unity tính theo đường kính
            currentWarningObject.transform.localScale = new Vector3(aoeRadius * 2, 0.01f, aoeRadius * 2);
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Khi đến thời điểm vung tay xuống đất
        if (stateInfo.normalizedTime >= damageTime && !hasDealtDamage)
        {
            PerformAoE(animator.transform);
            hasDealtDamage = true;
        }

        if (stateInfo.normalizedTime >= 1.0f)
        {
            animator.SetBool("isAoEAttacking", false);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        NavMeshAgent agent = animator.GetComponent<NavMeshAgent>();
        if (agent != null) agent.isStopped = false;

        // Dọn dẹp: Nếu vòng đỏ chưa bị xóa (ví dụ Boss bị choáng ngắt animation giữa chừng) thì xóa nó đi
        if (currentWarningObject != null)
        {
            Destroy(currentWarningObject);
        }
    }

    private void PerformAoE(Transform bossTransform)
    {
        // --- XÓA VÒNG ĐỎ VÀ TẠO HIỆU ỨNG NỔ ---
        if (currentWarningObject != null)
        {
            Destroy(currentWarningObject);
        }

        if (aoeEffectPrefab != null)
        {
            Instantiate(aoeEffectPrefab, bossTransform.position, Quaternion.identity);
        }

        // Quét sát thương
        Collider[] hitColliders = Physics.OverlapSphere(bossTransform.position, aoeRadius);
        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Player"))
            {
                PlayerState.Instance.TakeDamage(aoeDamage);
                Debug.Log("Player không né kịp vòng đỏ và dính đòn AoE!");
            }
        }
    }
}