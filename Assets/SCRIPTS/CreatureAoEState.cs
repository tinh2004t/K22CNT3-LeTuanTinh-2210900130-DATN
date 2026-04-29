using UnityEngine;
using UnityEngine.AI;

public class CreatureAoEState : StateMachineBehaviour
{
    [Header("Cấu hình đòn AoE")]
    public float aoeRadius = 5f;
    public int aoeDamage = 30;
    public float damageTime = 0.5f;

    [Header("Hiệu ứng & Cảnh báo")]
    public GameObject warningPrefab;
    public GameObject aoeEffectPrefab;

    private bool hasDealtDamage;
    private GameObject currentWarningObject;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hasDealtDamage = false;

        NavMeshAgent agent = animator.GetComponent<NavMeshAgent>();
        if (agent != null) agent.isStopped = true;

        if (warningPrefab != null)
        {
            Vector3 spawnPos = animator.transform.position + Vector3.up * 0.1f;
            currentWarningObject = Instantiate(warningPrefab, spawnPos, warningPrefab.transform.rotation);

            currentWarningObject.transform.localScale = new Vector3(aoeRadius * 2, 0.01f, aoeRadius * 2);
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
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

        if (currentWarningObject != null)
        {
            Destroy(currentWarningObject);
        }
    }

    private void PerformAoE(Transform bossTransform)
    {
        if (currentWarningObject != null)
        {
            Destroy(currentWarningObject);
        }

        if (aoeEffectPrefab != null)
        {
            Instantiate(aoeEffectPrefab, bossTransform.position, Quaternion.identity);
        }

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