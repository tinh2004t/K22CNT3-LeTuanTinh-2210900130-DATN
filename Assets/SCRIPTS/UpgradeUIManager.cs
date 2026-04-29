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
    public Transform contentTransform;    
    public GameObject upgradeSlotPrefab;  
    public Button exitButton;

    void Start()
    {
        exitButton.onClick.AddListener(ExitUpgradeMode);
    }

    public void OpenMenu(List<UpgradeRecipeSO> recipes)
    {
        upgradePanelUI.SetActive(true);

        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }

        foreach (var recipe in recipes)
        {
            GameObject prefab = Instantiate(upgradeSlotPrefab, contentTransform);
            UpgradeItemSlot slot = prefab.GetComponent<UpgradeItemSlot>();
            slot.recipe = recipe;

            InventoryItem upgradedItemInfo = recipe.upgradedItemPrefab.GetComponent<InventoryItem>();
            InventoryItem baseItemInfo = recipe.baseItemPrefab.GetComponent<InventoryItem>();

            slot.itemNameUI.text = upgradedItemInfo.thisName;
            slot.itemImageUI.sprite = recipe.upgradedItemPrefab.GetComponent<Image>().sprite;

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

    public void AttemptUpgrade(UpgradeRecipeSO recipe)
    {
        string baseName = recipe.baseItemPrefab.GetComponent<InventoryItem>().thisName;
        string upgradedName = recipe.upgradedItemPrefab.GetComponent<InventoryItem>().thisName;

        print($"--- ĐANG THỬ NÂNG CẤP: {baseName} lên {upgradedName} ---");

        if (InventorySystem.Instance.currentCoins < recipe.coinCost)
        {
            print($"[Lỗi Nâng Cấp]: Không đủ tiền vàng! Yêu cầu: {recipe.coinCost}, Hiện có: {InventorySystem.Instance.currentCoins}");
            return;
        }

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

        print("Đủ điều kiện! Đang tiến hành rèn...");

        InventorySystem.Instance.currentCoins -= recipe.coinCost;
        InventorySystem.Instance.RemoveItem(baseName, 1);

        foreach (var mat in recipe.requiredMaterials)
        {
            InventorySystem.Instance.RemoveItem(mat.itemName, mat.amount);
        }

        InventorySystem.Instance.AddToInventory(upgradedName);

        print($"[Thành công]: Chúc mừng bạn đã nhận được {upgradedName}!");

        ExitUpgradeMode();
    }

    internal void CloseMenu()
    {
        upgradePanelUI.SetActive(false);

    }
}