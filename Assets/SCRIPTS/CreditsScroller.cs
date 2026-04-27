using UnityEngine;

public class CreditsScroller : MonoBehaviour
{
    [Header("Cài đặt tốc độ")]
    [Tooltip("Tốc độ chạy chữ (số càng to chạy càng nhanh)")]
    public float scrollSpeed = 80f;

    private RectTransform textRectTransform;

    void Start()
    {
        // Lấy component RectTransform của chính object chứa script này
        textRectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // Đẩy vị trí chữ lên trên đều đặn theo thời gian
        if (textRectTransform != null)
        {
            textRectTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
        }
    }
}