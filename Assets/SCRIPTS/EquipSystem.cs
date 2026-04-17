using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class EquipSystem : MonoBehaviour
{
    public static EquipSystem Instance { get; set; }

    // -- UI -- //
    public GameObject quickSlotsPanel;

    public List<GameObject> quickSlotsList = new List<GameObject>();

    public GameObject numbersHolder;

    public int selectedNumber = -1;
    public GameObject selectedItem;


    public GameObject toolHolder;

    public GameObject selectedItemModel;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    private void Start()
    {
        PopulateSlotList();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectQuickSlot(1);
        } else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectQuickSlot(2);
        } else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectQuickSlot(3);
        } else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectQuickSlot(4);
        } else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectQuickSlot(5);
        } else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SelectQuickSlot(6);
        } else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SelectQuickSlot(7);
        }
    }

    void SelectQuickSlot (int number)
    {
        if (checkIfSlotIsFull(number) == true)
        {
            if (selectedNumber != number)
            {
                selectedNumber = number;



                // Unselect previous item
                if (selectedItem != null)
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                }


                selectedItem = getSelectedItem(number);
                selectedItem.GetComponent<InventoryItem>().isSelected = true;

                SetEquippedModel(selectedItem);



                //Changing the color
                foreach (Transform child in numbersHolder.transform)
                {
                    child.transform.Find("Text").GetComponent<TMP_Text>().color = Color.gray;
                }

                TMP_Text toBeChanged = numbersHolder.transform.Find("number" + number).transform.Find("Text").GetComponent<TMP_Text>();
                toBeChanged.color = Color.white;
            }
            else // We are trying select the same slot
            {
                selectedNumber = -1; // null

                // Unselect previous item
                if (selectedItem != null)
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                    selectedItem = null;
                }

                if (selectedItemModel != null)
                {
                    DestroyImmediate(selectedItemModel.gameObject);
                    selectedItemModel = null;
                }

                //Changing the color
                foreach (Transform child in numbersHolder.transform)
                {
                    child.transform.Find("Text").GetComponent<TMP_Text>().color = Color.gray;
                }

            }
        }
        
    }

    private void SetEquippedModel(GameObject selectedItem)
    {
        if (selectedItemModel != null)
        {
            DestroyImmediate(selectedItemModel.gameObject);
            selectedItemModel = null;
        }

        // Cắt bỏ chữ (Clone) và các khoảng trắng thừa
        string selectedItemName = selectedItem.name.Replace("(Clone)", "").Trim();

        string modelName = CalculateItemModel(selectedItemName);

        // NẾU VẬT PHẨM KHÔNG CÓ MODEL (modelName bị null), THÌ NGỪNG LẠI KHÔNG INSTANTIATE NỮA
        if (string.IsNullOrEmpty(modelName))
        {
            return;
        }

        // Load prefab từ Resources
        GameObject modelPrefab = Resources.Load<GameObject>(modelName);

        // KIỂM TRA NULL LẦN CUỐI TRƯỚC KHI TẠO
        if (modelPrefab != null)
        {
            selectedItemModel = Instantiate(modelPrefab);
            selectedItemModel.transform.SetParent(toolHolder.transform, false);
        }
        else
        {
            Debug.LogWarning($"Không tìm thấy Prefab nào tên là '{modelName}' trong thư mục Resources!");
        }
    }

    private string CalculateItemModel(string selectedItemName)
    {
        switch (selectedItemName)
        {
            case "Axe":
                return "Axe_Model";
            case "Tomato Seed":
                return "Hand_Model";
            case "Pumpkin Seed":
                return "Hand_Model";
            case "Watering Can":
                return "WateringCan_Model";
            default:
                return null;
        }
    }

    GameObject getSelectedItem(int slotNumber)
    {
        return quickSlotsList[slotNumber-1].transform.GetChild(0).gameObject;
    }

    bool checkIfSlotIsFull(int slotNumber)
    {
        if (quickSlotsList[slotNumber-1].transform.childCount > 0)
        {
            return true;
        } else
        {
            return false;
        }
    }

    private void PopulateSlotList()
    {
        foreach (Transform child in quickSlotsPanel.transform)
        {
            if (child.CompareTag("QuickSlot"))
            {
                quickSlotsList.Add(child.gameObject);
            }
        }
    }

    public void AddToQuickSlots(GameObject itemToEquip)
    {
        // Find next free slot
        GameObject availableSlot = FindNextEmptySlot();
        // Set transform of our object
        itemToEquip.transform.SetParent(availableSlot.transform, false);

        InventorySystem.Instance.ReCalculateList();

    }


    public GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject();
    }

    public bool CheckIfFull()
    {

        int counter = 0;

        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount > 0)
            {
                counter += 1;
            }
        }

        if (counter == 7)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    internal bool IsHoldingWeapon()
    {
        if (selectedItem != null)
        {
            Weapon weapon = selectedItem.GetComponent<Weapon>();
            if (weapon != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    internal int GetWeaponDamage()
    {
        if (selectedItem != null)
        {
            return selectedItem.GetComponent<Weapon>().weaponDamage;
            
        }
        else
        {
            return 0;
        }
    }

    public bool IsPlayerHoldingSeed()
    {
        if (selectedItemModel != null)
        {
            switch (selectedItemModel.gameObject.name)
            {
                case "Hand_Model(Clone)":
                    return true;
                case "Hand_Model":
                    return true;
                default:
                    return false;
            }
        }
        else
        {
            return false;
        }
    }

    internal bool IsPlayerHoldingWateringCan()
    {
        if (selectedItem != null)
        {
            switch (selectedItem.gameObject.name)
            {
                case "Watering Can(Clone)":
                    Console.WriteLine("Player is holding watering can");
                    return true;
                
                default:
                    return false;
            }
        }
        else
        {
            return false;
        }
    }
}