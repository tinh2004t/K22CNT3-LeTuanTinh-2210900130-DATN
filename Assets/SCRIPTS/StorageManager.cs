using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageManager : MonoBehaviour
{
    public static StorageManager Instance { get; set; }

    [SerializeField] GameObject storageBoxSmallUI;
    [SerializeField] StorageBox selectedStorage;
    public bool storageUIOpen;

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

    public void OpenBox(StorageBox storage)
    {
        SetSelectedStorage(storage);

        PopulateStorage(GetRelevantUI(selectedStorage));

        GetRelevantUI(selectedStorage).SetActive(true);
        storageUIOpen = true;

        MovementManager.Instance.EnableMovement(false);
        MovementManager.Instance.EnableLook(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SelectionManager.Instance.DisableSelection();
        SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;
    }

    private void PopulateStorage(GameObject storageUI)
    {
        // Get all slots of the ui
        List<GameObject> uiSlots = new List<GameObject>();

        foreach (Transform child in storageUI.transform)
        {
            uiSlots.Add(child.gameObject);
        }

        // Now, instantiate the prefab and set it as a child of each GameObject
        foreach (string name in selectedStorage.items)
        {
            foreach (GameObject slot in uiSlots)
            {
                if (slot.transform.childCount < 1)
                {
                    var itemToAdd = Instantiate(Resources.Load<GameObject>(name), slot.transform.position, slot.transform.rotation);

                    itemToAdd.name = name;

                    itemToAdd.transform.SetParent(slot.transform);
                    break;
                }
            }
        }
    }

    public void CloseBox()
    {
        // Gọi hàm tính toán lại (không cần truyền tham số vì ta dùng selectedStorage global)
        RecaculateStorage();

        GetRelevantUI(selectedStorage).SetActive(false);
        storageUIOpen = false;

        MovementManager.Instance.EnableMovement(true);
        MovementManager.Instance.EnableLook(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SelectionManager.Instance.EnableSelection();
        SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;

        // Reset selectedStorage để an toàn
        selectedStorage = null;
    }

    private void RecaculateStorage()
    {
        // 1. Lấy tham chiếu đến cái UI Panel đang mở (nơi chứa các Slot)
        GameObject openUI = GetRelevantUI(selectedStorage);

        // 2. Lấy danh sách các Slot trong UI
        List<GameObject> uiSlots = new List<GameObject>();
        foreach (Transform child in openUI.transform)
        {
            uiSlots.Add(child.gameObject);
        }

        // 3. Xóa dữ liệu cũ trong rương để cập nhật mới
        selectedStorage.items.Clear();

        // 4. Quét qua từng Slot trên UI để xem có item nào không
        foreach (GameObject slot in uiSlots)
        {
            if (slot.transform.childCount > 0)
            {
                // Lấy item con của slot (Item vừa được kéo vào)
                GameObject itemObj = slot.transform.GetChild(0).gameObject;

                string name = itemObj.name;
                string str2 = "(Clone)";
                string result = name.Replace(str2, "");

                // 5. Lưu tên item vào danh sách data của rương
                selectedStorage.items.Add(result);

                // QUAN TRỌNG: Sau khi lưu xong, cần xóa vật thể UI đi để lần sau mở rương không bị chồng chéo
                Destroy(itemObj);
            }
        }
    }

    public void SetSelectedStorage(StorageBox storage)
    {
        selectedStorage = storage;
    }

    private GameObject GetRelevantUI(StorageBox storage)
    {
        // Create a switch for other types
        return storageBoxSmallUI;
    }
}