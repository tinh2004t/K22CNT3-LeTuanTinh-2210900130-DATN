using UnityEngine;
using System.Collections;

public class BossAreaTeleporter : MonoBehaviour
{
    [Header("Cấu hình dịch chuyển")]
    public Transform destination;
    public GameObject vfxEffect;

    [Header("Điều kiện dịch chuyển")]
    public string requiredItemName = "The Genesis Core";
    public bool consumeKeyOnUse = true;

    [Header("Cấu hình UI Thông báo")]
    [Tooltip("Kéo thả GameObject chứa Text thông báo vào đây")]
    public GameObject warningMessageUI;
    [Tooltip("Thời gian hiển thị thông báo (giây)")]
    public float messageDisplayTime = 3f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (HasRequiredItem())
            {
                TeleportPlayer(other.gameObject);
            }
            else
            {
                Debug.Log($"Bạn không thể vào! Cần có vật phẩm: {requiredItemName}");

                ShowWarningUI();
            }
        }
    }

    private void ShowWarningUI()
    {
        if (warningMessageUI != null)
        {
            StopAllCoroutines();
            StartCoroutine(DisplayWarningRoutine());
        }
        else
        {
            Debug.LogWarning("Bạn chưa gán warningMessageUI trong Inspector!");
        }
    }

    private IEnumerator DisplayWarningRoutine()
    {
        warningMessageUI.SetActive(true); 
        yield return new WaitForSeconds(messageDisplayTime); 
        warningMessageUI.SetActive(false); 
    }

    private bool HasRequiredItem()
    {
        if (InventorySystem.Instance != null)
        {
            if (InventorySystem.Instance.CheckItemAmount(requiredItemName) > 0)
            {
                if (consumeKeyOnUse)
                {
                    InventorySystem.Instance.RemoveItem(requiredItemName, 1);
                    Debug.Log($"Đã tiêu hao 1 {requiredItemName} để mở cổng Boss!");
                }
                return true;
            }
        }
        else
        {
            Debug.LogError("Lỗi: Không tìm thấy InventorySystem trong Scene!");
        }

        return false;
    }

    void TeleportPlayer(GameObject player)
    {
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        player.transform.position = destination.position;
        player.transform.rotation = destination.rotation;

        if (cc != null) cc.enabled = true;

        if (vfxEffect != null)
        {
            Instantiate(vfxEffect, destination.position, Quaternion.identity);
        }

        Debug.Log("Đã đưa người chơi vào đấu trường Boss!");
    }
}