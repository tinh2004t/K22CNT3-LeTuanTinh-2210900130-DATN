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
<<<<<<< HEAD
    public Transform contentTransform;    
    public GameObject upgradeSlotPrefab;  
=======
    public Transform contentTransform;    // Nơi chứa các slot
    public GameObject upgradeSlotPrefab;  // Kéo Prefab vừa tạo ở Bước 2 vào đây
>>>>>>> origin/main
    public Button exitButton;

    void Start()
    {
        exitButton.onClick.AddListener(ExitUpgradeMode);
    }

<<<<<<< HEAD
=======
    // Hàm này sẽ sinh các UI Slot ra màn hình
>>>>>>> origin/main
    public void OpenMenu(List<UpgradeRecipeSO> recipes)
    {
        upgradePanelUI.SetActive(true);

<<<<<<< HEAD
=======
        // Xóa các slot cũ (nếu có) để không bị trùng lặp
>>>>>>> origin/main
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }

<<<<<<< HEAD
=======
        // Tạo danh sách mới
>>>>>>> origin/main
        foreach (var recipe in recipes)
        {
            GameObject prefab = Instantiate(upgradeSlotPrefab, contentTransform);
            UpgradeItemSlot slot = prefab.GetComponent<UpgradeItemSlot>();
            slot.recipe = recipe;

<<<<<<< HEAD
            InventoryItem upgradedItemInfo = recipe.upgradedItemPrefab.GetComponent<InventoryItem>();
            InventoryItem baseItemInfo = recipe.baseItemPrefab.GetComponent<InventoryItem>();

            slot.itemNameUI.text = upgradedItemInfo.thisName;
            slot.itemImageUI.sprite = recipe.upgradedItemPrefab.GetComponent<Image>().sprite;

=======
            // Lấy dữ liệu từ GameObject (Y hệt cách bạn làm trong BuySystem)
            InventoryItem upgradedItemInfo = recipe.upgradedItemPrefab.GetComponent<InventoryItem>();
            InventoryItem baseItemInfo = recipe.baseItemPrefab.GetComponent<InventoryItem>();

            // Gán thông tin hiển thị lên UI
            slot.itemNameUI.text = upgradedItemInfo.thisName;
            slot.itemImageUI.sprite = recipe.upgradedItemPrefab.GetComponent<Image>().sprite;

            // Ghi text nguyên liệu cần thiết
>>>>>>> origin/main
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

<<<<<<< HEAD
=======
    // --- HÀM XỬ LÝ TRỪ ĐỒ/ THÊM ĐỒ (Logic ở tin nhắn trước) ---
>>>>>>> origin/main
    public void AttemptUpgrade(UpgradeRecipeSO recipe)
    {
        string baseName = recipe.baseItemPrefab.GetComponent<InventoryItem>().thisName;
        string upgradedName = recipe.upgradedItemPrefab.GetComponent<InventoryItem>().thisName;

        print($"--- ĐANG THỬ NÂNG CẤP: {baseName} lên {upgradedName} ---");

<<<<<<< HEAD
=======
        // 1. Kiểm tra tiền
>>>>>>> origin/main
        if (InventorySystem.Instance.currentCoins < recipe.coinCost)
        {
            print($"[Lỗi Nâng Cấp]: Không đủ tiền vàng! Yêu cầu: {recipe.coinCost}, Hiện có: {InventorySystem.Instance.currentCoins}");
            return;
        }

<<<<<<< HEAD
=======
        // 2. Kiểm tra đồ gốc trong túi
>>>>>>> origin/main
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

<<<<<<< HEAD
=======
        // 3. Kiểm tra từng loại nguyên liệu
>>>>>>> origin/main
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

<<<<<<< HEAD
=======
        // --- NẾU VƯỢ QUA HẾT CÁC BƯỚC TRÊN THÌ BẮT ĐẦU TRỪ ĐỒ ---
>>>>>>> origin/main
        print("Đủ điều kiện! Đang tiến hành rèn...");

        InventorySystem.Instance.currentCoins -= recipe.coinCost;
        InventorySystem.Instance.RemoveItem(baseName, 1);

        foreach (var mat in recipe.requiredMaterials)
        {
            InventorySystem.Instance.RemoveItem(mat.itemName, mat.amount);
        }

        InventorySystem.Instance.AddToInventory(upgradedName);

        print($"[Thành công]: Chúc mừng bạn đã nhận được {upgradedName}!");

<<<<<<< HEAD
=======
        // Đóng giao diện sau khi rèn xong
>>>>>>> origin/main
        ExitUpgradeMode();
    }

    internal void CloseMenu()
    {
        upgradePanelUI.SetActive(false);

    }
}