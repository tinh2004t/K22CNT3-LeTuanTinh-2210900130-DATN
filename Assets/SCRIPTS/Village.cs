using UnityEngine;

public class Village : MonoBehaviour
{
    [Header("Checkpoint")]
    public Checkpoint reachVillage_Dana;

    [Header("Quest Settings")]
    public NPC npcDana;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (reachVillage_Dana != null && !reachVillage_Dana.isCompleted)
            {
                reachVillage_Dana.isCompleted = true;
                if (QuestManager.Instance != null) QuestManager.Instance.RefreshTrackerList();
            }

            if (npcDana != null)
            {
                if (npcDana.currentActiveQuest.questName == "The survivors"
                    && !npcDana.currentActiveQuest.isCompleted)
                {
                    Debug.Log("Đã tới làng -> Gọi NPC trả thưởng!");

                    npcDana.ReceiveRewardAndCompleteQuest();
                }
            }
            else
            {
                Debug.LogError("LỖI: Bạn chưa kéo script NPC của Dana vào ô 'Npc Dana' trong Inspector của Village!");
            }
        }
    }
}