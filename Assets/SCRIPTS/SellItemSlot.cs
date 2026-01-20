using UnityEngine;
using UnityEngine.EventSystems;

// LỖI Ở ĐÂY: Bạn phải thêm ", IDropHandler" vào sau MonoBehaviour
public class SellItemSlot : MonoBehaviour, IDropHandler
{
    public GameObject currentItem;

    // Hàm này CHỈ CHẠY khi class có kế thừa IDropHandler
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop vào SellSlot: " + gameObject.name);

        GameObject droppedItem = DragDrop.itemBeingDragged;

        if (droppedItem != null)
        {
            // 1. Đặt item vào trong slot
            droppedItem.transform.SetParent(transform);
            droppedItem.transform.localPosition = Vector3.zero;

            // 2. Ngắt kết nối với DragDrop để nó không tự vứt đồ ra đất
            DragDrop.itemBeingDragged = null;

            // 3. Cập nhật dữ liệu
            currentItem = droppedItem;

            // 4. Tính lại tổng tiền
            if (SellSystem.Instance != null)
            {
                SellSystem.Instance.CalculateTotalSellPrice();
            }
        }
    }

    public bool IsEmpty()
    {
        return transform.childCount == 0;
    }

    void Update()
    {
        // Kiểm tra nếu item bị kéo ra khỏi slot thì trừ tiền đi
        if (currentItem != null && currentItem.transform.parent != transform)
        {
            currentItem = null;
            if (SellSystem.Instance != null)
            {
                SellSystem.Instance.CalculateTotalSellPrice();
            }
        }
    }
}