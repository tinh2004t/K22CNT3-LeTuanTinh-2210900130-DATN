using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{

    public static InventorySystem Instance { get; set; }

    private Coroutine hideCoroutine;

    public GameObject inventoryScreenUI;

    public List<GameObject> slotList = new List<GameObject>();

    public List<string> itemsPickedup;

    public List<string> itemList = new List<string>();

    private GameObject itemToAdd;

    private GameObject whatSlotToEquip;

    public bool isOpen;

    //public bool isFull;

    //Pickup Pop Up

    public GameObject pickupAlert;
    public TMPro.TMP_Text pickupName;
    public Image pickupImage;

    public GameObject ItemInfoUi;



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


    void Start()
    {
        isOpen = false;
        PopulateSlotList();


        Cursor.visible = false;
    }

    void PopulateSlotList()
    {
        foreach (Transform child in inventoryScreenUI.transform)
        {
            if(child.CompareTag("Slot"))
            {
                slotList.Add(child.gameObject);
            }
        }
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.I) && !isOpen && !ConstructionManager.Instance.inConstructionMode)
        {

            inventoryScreenUI.SetActive(true);

            inventoryScreenUI.GetComponentInParent<Canvas>().sortingOrder = MenuManager.Instance.SetAsFront();

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;


            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;


            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            inventoryScreenUI.SetActive(false);
            if (!CraftingSystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                SelectionManager.Instance.EnableSelection();
                SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
            }
            isOpen = false;
        }
    }



    public void AddToInventory(string itemName)
    {
        if (!SaveManager.Instance.isLoading)
        {
        SoundManager.Instance.PlaySound(SoundManager.Instance.pickUpItemSound);

        }


        whatSlotToEquip = FindNextEmptySlot();
        itemToAdd = (GameObject)Instantiate(Resources.Load<GameObject>(itemName),whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
        itemToAdd.transform.SetParent(whatSlotToEquip.transform);

        itemList.Add(itemName);

        TriggerPickupPopUp(itemName, itemToAdd.GetComponent<Image>().sprite);




        ReCalculeList();
        CraftingSystem.Instance.RefreshNeededItems();
        QuestManager.Instance.RefreshTrackerList();

    }

    void TriggerPickupPopUp(string itemName, Sprite itemSprite)
    {
        // 1. C?p nh?t thông tin UI
        pickupAlert.SetActive(true);
        pickupName.text = itemName;
        pickupImage.sprite = itemSprite;

        // 2. N?u ?ang có b? ??m ng??c c? (do nh?t liên ti?p), hãy h?y nó ?i
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        // 3. B?t ??u b? ??m ng??c m?i 4 giây
        hideCoroutine = StartCoroutine(HidePopupAfterDelay(4f));
    }

    // Hàm Coroutine ?? ??m ng??c và t?t
    IEnumerator HidePopupAfterDelay(float delay)
    {
        // Ch? 4 giây (theo th?i gian game)
        yield return new WaitForSeconds(delay);

        // Sau khi ch? xong thì t?t Popup
        pickupAlert.SetActive(false);

        // Reset bi?n coroutine v? null
        hideCoroutine = null;
    }



    public bool CheckSlotsAvailable(int emptyNeeded)
    {
        int emptySlot = 0;
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount <= 0)
            {
                emptySlot++;
            }
 
        }
        if (emptySlot >= emptyNeeded)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    private GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }

        }
        return new GameObject();

    }

    public void RemoveItem( string nameToRemove, int amountToRemove)
    {
        int counter = amountToRemove;
        for (var i = slotList.Count - 1; i >= 0; i--)
        {
            if (slotList[i].transform.childCount >0)
            {
                if (slotList[i].transform.GetChild(0).name == nameToRemove + "(Clone)" && counter != 0)
                {
                    Destroy(slotList[i].transform.GetChild(0).gameObject);

                    counter--;

                }

            }
        }

        ReCalculeList();
        CraftingSystem.Instance.RefreshNeededItems();

        QuestManager.Instance.RefreshTrackerList();

    }

    public void ReCalculeList()
    {
        itemList.Clear();

        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                string name = slot.transform.GetChild(0).name;

                string str2 = "(Clone)";
                string result = name.Replace(str2, "");

                itemList.Add(result);
            }

        }
    }


    public int CheckItemAmount(string name)
    {
        int itemCounter = 0;

        foreach (string item in itemList)
        {
            if (item == name)
            {
                itemCounter++;
            }
        }
        return itemCounter;
    }


}

