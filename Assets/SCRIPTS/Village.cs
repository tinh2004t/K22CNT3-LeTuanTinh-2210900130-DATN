using UnityEngine;

public class Village : MonoBehaviour
{
    [Header("Checkpoint")]
    public Checkpoint reachVillage_Dana;

    [Header("Quest Settings")]
    public NPC npcDana; // 1. Tạo biến để chứa tham chiếu đến NPC Dana

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Xử lý Checkpoint
            if (reachVillage_Dana != null && !reachVillage_Dana.isCompleted)
            {
                reachVillage_Dana.isCompleted = true;
                // Cập nhật hiển thị tracker nếu cần
                if (QuestManager.Instance != null) QuestManager.Instance.RefreshTrackerList();
            }

            // Xử lý Hoàn thành nhiệm vụ thông qua NPC
            if (npcDana != null)
            {
                // Kiểm tra xem NPC có đang nắm giữ nhiệm vụ này không và nó đã xong chưa
                // Lưu ý: Phải kiểm tra đúng tên nhiệm vụ để tránh hoàn thành nhầm nhiệm vụ khác
                if (npcDana.currentActiveQuest.questName == "Beginning the journey"
                    && !npcDana.currentActiveQuest.isCompleted)
                {
                    Debug.Log("Đã tới làng -> Gọi NPC trả thưởng!");

                    // GỌI HÀM CỦA NPC TẠI ĐÂY
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