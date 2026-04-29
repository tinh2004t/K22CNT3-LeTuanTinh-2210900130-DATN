using UnityEngine;

public class CreditsScroller : MonoBehaviour
{
    [Header("Cài đặt tốc độ")]
    [Tooltip("Tốc độ chạy chữ (số càng to chạy càng nhanh)")]
    public float scrollSpeed = 80f;

    private RectTransform textRectTransform;

    void Start()
    {
        textRectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (textRectTransform != null)
        {
            textRectTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
        }
    }
}