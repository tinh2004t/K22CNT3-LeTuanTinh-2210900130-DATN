using UnityEngine;

public class BossAreaTeleporter : MonoBehaviour
{
    [Header("Cấu hình dịch chuyển")]
    public Transform destination; // Kéo BossArena_SpawnPoint vào đây
    public GameObject vfxEffect; // Hiệu ứng biến mất (tùy chọn)

    [Header("Điều kiện dịch chuyển")]
    public string requiredItemName = "The Genesis Core"; // Tên vật phẩm yêu cầu (Phải gõ chính xác tên Prefab/Item)
    public bool consumeKeyOnUse = true; // Chìa khóa có bị mất sau khi dùng không?

    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra nếu vật thể chạm vào là Player
        if (other.CompareTag("Player"))
        {
            if (HasRequiredItem())
            {
                TeleportPlayer(other.gameObject);
            }
            else
            {
                // Thông báo khi không đủ điều kiện
                Debug.Log($"Bạn không thể vào! Cần có vật phẩm: {requiredItemName}");

                // Bạn có thể kích hoạt một UI Text trên màn hình tại đây nếu muốn
                // Ví dụ: UIManager.Instance.ShowMessage("Cần có " + requiredItemName);
            }
        }
    }

    /// <summary>
    /// Kiểm tra và tiêu hao vật phẩm từ InventorySystem của bạn
    /// </summary>
    private bool HasRequiredItem()
    {
        // Kiểm tra xem hệ thống Inventory có đang tồn tại trong Scene không
        if (InventorySystem.Instance != null)
        {
            // Kiểm tra xem số lượng vật phẩm yêu cầu trong túi đồ có lớn hơn 0 không
            if (InventorySystem.Instance.CheckItemAmount(requiredItemName) > 0)
            {
                // Nếu cấu hình yêu cầu tiêu hao chìa khóa
                if (consumeKeyOnUse)
                {
                    InventorySystem.Instance.RemoveItem(requiredItemName, 1);
                    Debug.Log($"Đã tiêu hao 1 {requiredItemName} để mở cổng Boss!");
                }

                return true; // Đủ điều kiện
            }
        }
        else
        {
            Debug.LogError("Lỗi: Không tìm thấy InventorySystem trong Scene!");
        }

        return false; // Không đủ điều kiện (hoặc không có item)
    }

    void TeleportPlayer(GameObject player)
    {
        // Tắt CharacterController trước khi dời vị trí để tránh bị lỗi giật ngược (rubber banding)
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        // Dịch chuyển vị trí và góc quay
        player.transform.position = destination.position;
        player.transform.rotation = destination.rotation;

        if (cc != null) cc.enabled = true;

        // Tạo hiệu ứng tại điểm đến nếu có
        if (vfxEffect != null)
        {
            Instantiate(vfxEffect, destination.position, Quaternion.identity);
        }

        Debug.Log("Đã đưa người chơi vào đấu trường Boss!");
    }
}