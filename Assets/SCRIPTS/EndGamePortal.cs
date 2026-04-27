using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

public class EndGamePortal : MonoBehaviour
{
    // --- THÊM BIẾN NÀY ---
    // Biến static để các script khác (như túi đồ) có thể dễ dàng kiểm tra
    public static bool isCutscenePlaying = false;
    // ----------------------

    [Header("Cấu hình Cutscene")]
    [Tooltip("Kéo PlayableDirector chứa Timeline End Game vào đây")]
    public PlayableDirector endCutsceneDirector;

    [Header("Điều kiện kết game")]
    public string requiredItemName = "Heaven's key";
    [Tooltip("Có xóa chìa khóa sau khi dùng không?")]
    public bool consumeKeyOnUse = false;

    [Header("Cấu hình UI Thông báo")]
    [Tooltip("Kéo thả GameObject chứa Text thông báo vào đây")]
    public GameObject warningMessageUI;
    [Tooltip("Thời gian hiển thị thông báo (giây)")]
    public float messageDisplayTime = 3f;

    private bool hasTriggered = false;

    private void Start()
    {
        // Đảm bảo khi mới vào game, biến này luôn ở trạng thái false
        isCutscenePlaying = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            if (HasRequiredItem())
            {
                PlayEndGameCutscene();
                PrepareEndGame();
            }
            else
            {
                Debug.Log($"Bạn không thể kết thúc game! Cần có vật phẩm: {requiredItemName}");
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
                    Debug.Log($"Đã tiêu hao 1 {requiredItemName} để kích hoạt End Game!");
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

    private void PlayEndGameCutscene()
    {
        hasTriggered = true;
        Debug.Log("Đã đủ điều kiện! Đang phát Cutscene End Game...");

        if (endCutsceneDirector != null)
        {
            endCutsceneDirector.Play();
        }
        else
        {
            Debug.LogWarning("Chưa gán PlayableDirector vào EndGamePortal!");
        }
    }

    public void PrepareEndGame()
    {
        // --- CẬP NHẬT TRẠNG THÁI ---
        isCutscenePlaying = true;
        MovementManager.Instance.canMove = false;
        MovementManager.Instance.EnableLook(false);

        // Bật cờ báo hiệu Cutscene đang chạy
        // ---------------------------

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            if (player.TryGetComponent(out CharacterController cc)) cc.enabled = false;
            Debug.Log("Đã khóa Player");
        }

        GameObject mainCanvas = GameObject.Find("Canvas");
        if (mainCanvas != null)
        {
            Transform statusBar = mainCanvas.transform.Find("StatusBarArea");
            if (statusBar != null) statusBar.gameObject.SetActive(false);

            Transform reticle = mainCanvas.transform.Find("Middle Reticle");
            if (reticle != null) reticle.gameObject.SetActive(false);

            Transform interactUI = mainCanvas.transform.Find("Interaction_Info_UI");
            if (interactUI != null) interactUI.gameObject.SetActive(false);
        }

        GameObject npcContainer = GameObject.Find("NPC");
        if (npcContainer != null) npcContainer.SetActive(false);

        Debug.Log("Đã vô hiệu hóa toàn bộ hệ thống trò chơi để chạy Cutscene.");
    }
}