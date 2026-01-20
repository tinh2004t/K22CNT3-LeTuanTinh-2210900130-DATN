using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SellSystem : MonoBehaviour
{
    public static SellSystem Instance { get; set; }

    [Header("UI References")]
    public TextMeshProUGUI totalMoneyText;
    public Button sellButton;
    public Button backButton;

    [Header("Slots")]
    public List<SellItemSlot> sellSlots;

    private int currentTotalSellValue = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    private void Start()
    {
        sellButton.onClick.AddListener(SellAllItems);
        // Gán sự kiện cho nút back nếu cần
        if (backButton != null) backButton.onClick.AddListener(CloseSellPanel);
    }

    public void CalculateTotalSellPrice()
    {
        currentTotalSellValue = 0;

        foreach (SellItemSlot slot in sellSlots)
        {
            if (slot.transform.childCount > 0)
            {
                InventoryItem itemData = slot.transform.GetChild(0).GetComponent<InventoryItem>();
                if (itemData != null)
                {
                    currentTotalSellValue += itemData.sellPrice;
                }
            }
        }

        if (totalMoneyText != null)
            totalMoneyText.text = "Total: " + currentTotalSellValue.ToString();

        if (sellButton != null)
            sellButton.interactable = currentTotalSellValue > 0;
    }

    public void SellAllItems()
    {
        if (currentTotalSellValue == 0) return;

        // 1. Cộng tiền
        InventorySystem.Instance.currentCoins += currentTotalSellValue;
        Debug.Log("Đã bán tất cả với giá: " + currentTotalSellValue);

        // 2. Xóa vật phẩm khỏi Sell Slot
        foreach (SellItemSlot slot in sellSlots)
        {
            if (slot.transform.childCount > 0)
            {
                GameObject itemObj = slot.transform.GetChild(0).gameObject;
                Destroy(itemObj);
                slot.currentItem = null;
            }
        }

        // 3. Cập nhật dữ liệu Inventory
        InventorySystem.Instance.ReCalculateList();
        if (CraftingSystem.Instance != null)
        {
            CraftingSystem.Instance.RefreshNeededItems();
        }

        // --- SỬA LỖI TẠI ĐÂY ---

        // Thay vì gọi CalculateTotalSellPrice() (vì Destroy chưa xong),
        // Ta ép buộc reset giá trị về 0 luôn.

        currentTotalSellValue = 0;

        if (totalMoneyText != null)
        {
            totalMoneyText.text = "Total: 0";
        }

        if (sellButton != null)
        {
            sellButton.interactable = false;
        }
    }

    void CloseSellPanel()
    {
        // Khi đóng panel, nếu còn đồ trong Sell Slot thì trả về Inventory
        // (Logic trả đồ bạn có thể làm sau, tạm thời chỉ ẩn panel)
        gameObject.SetActive(false);

        // Gọi lại ShopKeeper để hiện lại dialog (nếu cần)
        if (BuySystem.Instance != null && BuySystem.Instance.ShopKeeper != null)
        {
            BuySystem.Instance.ShopKeeper.DialogMode();
        }
    }
}