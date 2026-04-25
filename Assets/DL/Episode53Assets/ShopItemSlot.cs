using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static BuySystem;

public class ShopItemSlot : MonoBehaviour
{
    [Header("UI")]
    public Image itemImageUI;
    public TextMeshProUGUI itemNameUI;
    public TextMeshProUGUI itemPriceUI;
    public Button buyButtonUI;

    [Header("Data")]
    public ShopItemData shopItemData;

    private void Start()
    {
        buyButtonUI.onClick.AddListener(BuyItem);
    }

    // Update is called once per frame
    void Update()
    {
        //int currentMoney = 100; // Hardcoded, later get it from inventory
        int currentMoney = InventorySystem.Instance.currentCoins;

        // Enable/Disable Button based on current owned money
        if (shopItemData.itemPrice <= currentMoney) 
        {
            buyButtonUI.interactable = true;
        }
        else
        {
            buyButtonUI.interactable = false;
        }
    }

    public void BuyItem()
    {
        // Remove Coins
        InventorySystem.Instance.currentCoins -= shopItemData.itemPrice;

        // Get inventory item Name
        InventoryItem inventoryItem = shopItemData.inventoryItem.GetComponent<InventoryItem>();

        print("Bought " + inventoryItem.thisName);

        Debug.Log("Bought " + inventoryItem.thisName);
        // Add items into inventory
        InventorySystem.Instance.AddToInventory(inventoryItem.thisName);
        
    }
}
