using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUIManager : MonoBehaviour
{
    public static UpgradeUIManager Instance { get; set; }
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    [Header("References")]
    public BlacksmithNPC blacksmith;
    public GameObject upgradePanelUI;
    public Transform contentTransform;    // Nơi chứa các slot
    public GameObject upgradeSlotPrefab;  // Kéo Prefab vừa tạo ở Bước 2 vào đây
    public Button exitButton;

    void Start()
    {
        exitButton.onClick.AddListener(ExitUpgradeMode);
    }

    // Hàm này sẽ sinh các UI Slot ra màn hình
    public void OpenMenu(List<UpgradeRecipeSO> recipes)
    {
        upgradePanelUI.SetActive(true);

        // Xóa các slot cũ (nếu có) để không bị trùng lặp
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }

        // Tạo danh sách mới
        foreach (var recipe in recipes)
        {
            GameObject prefab = Instantiate(upgradeSlotPrefab, contentTransform);
            UpgradeItemSlot slot = prefab.GetComponent<UpgradeItemSlot>();
            slot.recipe = recipe;

            // Lấy dữ liệu từ GameObject (Y hệt cách bạn làm trong BuySystem)
            InventoryItem upgradedItemInfo = recipe.upgradedItemPrefab.GetComponent<InventoryItem>();
            InventoryItem baseItemInfo = recipe.baseItemPrefab.GetComponent<InventoryItem>();

            // Gán thông tin hiển thị lên UI
            slot.itemNameUI.text = upgradedItemInfo.thisName;
            slot.itemImageUI.sprite = recipe.upgradedItemPrefab.GetComponent<Image>().sprite;

            // Ghi text nguyên liệu cần thiết
            string requirements = $"Vàng: {recipe.coinCost}\n";
            requirements += $"- {baseItemInfo.thisName} (x1)\n";
            foreach (var mat in recipe.requiredMaterials)
            {
                requirements += $"- {mat.itemName} (x{mat.amount})\n";
            }
            slot.costTextUI.text = requirements;
        }
    }

    public void ExitUpgradeMode()
    {
        blacksmith.StopTalking();
    }

    // --- HÀM XỬ LÝ TRỪ ĐỒ/ THÊM ĐỒ (Logic ở tin nhắn trước) ---
    public void AttemptUpgrade(UpgradeRecipeSO recipe)
    {
        string baseName = recipe.baseItemPrefab.GetComponent<InventoryItem>().thisName;
        string upgradedName = recipe.upgradedItemPrefab.GetComponent<InventoryItem>().thisName;

        print($"--- ĐANG THỬ NÂNG CẤP: {baseName} lên {upgradedName} ---");

        // 1. Kiểm tra tiền
        if (InventorySystem.Instance.currentCoins < recipe.coinCost)
        {
            print($"[Lỗi Nâng Cấp]: Không đủ tiền vàng! Yêu cầu: {recipe.coinCost}, Hiện có: {InventorySystem.Instance.currentCoins}");
            return;
        }

        // 2. Kiểm tra đồ gốc trong túi
        int baseCount = 0;
        foreach (string item in InventorySystem.Instance.itemList)
        {
            if (item == baseName) baseCount++;
        }

        if (baseCount < 1)
        {
            print($"[Lỗi Nâng Cấp]: Không tìm thấy [{baseName}] trong túi đồ của bạn!");
            return;
        }

        // 3. Kiểm tra từng loại nguyên liệu
        foreach (var mat in recipe.requiredMaterials)
        {
            int matCount = 0;
            foreach (string item in InventorySystem.Instance.itemList)
            {
                if (item == mat.itemName) matCount++;
            }

            if (matCount < mat.amount)
            {
                print($"[Lỗi Nâng Cấp]: Không đủ [{mat.itemName}]! Yêu cầu: {mat.amount}, Hiện có: {matCount}");
                return;
            }
        }

        // --- NẾU VƯỢ QUA HẾT CÁC BƯỚC TRÊN THÌ BẮT ĐẦU TRỪ ĐỒ ---
        print("Đủ điều kiện! Đang tiến hành rèn...");

        InventorySystem.Instance.currentCoins -= recipe.coinCost;
        InventorySystem.Instance.RemoveItem(baseName, 1);

        foreach (var mat in recipe.requiredMaterials)
        {
            InventorySystem.Instance.RemoveItem(mat.itemName, mat.amount);
        }

        InventorySystem.Instance.AddToInventory(upgradedName);

        print($"[Thành công]: Chúc mừng bạn đã nhận được {upgradedName}!");

        // Đóng giao diện sau khi rèn xong
        ExitUpgradeMode();
    }

    internal void CloseMenu()
    {
        upgradePanelUI.SetActive(false);

    }
}