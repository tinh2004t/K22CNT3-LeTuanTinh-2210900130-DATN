using System.Collections.Generic;
using UnityEngine;

public class PlacebleItem : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float rotateSpeed = 10f; // Tốc độ xoay để tạo độ mượt
    [SerializeField] private LayerMask groundLayer;   // Layer của mặt đất

    [Header("Validation Status")]
    public bool isValidToBeBuilt;
    [SerializeField] bool isGrounded;
    [SerializeField] bool isOverlappingItems;

    [Header("Components")]
    [SerializeField] BoxCollider solidCollider;
    private Outline outline;

    // Biến lưu trữ số lượng vật thể đang va chạm (thay vì bool để tránh lỗi logic khi va chạm nhiều vật cùng lúc)
    private int overlapCount = 0;

    private void Start()
    {
        outline = GetComponent<Outline>();
        // Tự động gán Layer nếu chưa set, giả sử layer Ground là Default hoặc bạn tự set trong Inspector
        if (groundLayer == 0) groundLayer = LayerMask.GetMask("Ground", "Terrain");
    }

    void Update()
    {
        // 1. Kiểm tra Grounded và Xử lý xoay (Placement Logic)
        HandleGroundCheckAndRotation();

        // 2. Xác định trạng thái Valid
        // Valid khi: Chạm đất VÀ Không bị chồng lấn
        if (isGrounded && overlapCount <= 0)
        {
            isValidToBeBuilt = true;
            SetValidColor(); // Tự động đổi màu luôn ở đây cho tiện
        }
        else
        {
            isValidToBeBuilt = false;
            SetInvalidColor();
        }
    }

    private void HandleGroundCheckAndRotation()
    {
        if (PlacementSystem.Instance != null && !PlacementSystem.Instance.inPlacementMode) return;

        // 1. Xác định chiều cao thực tế của Object dựa trên Collider
        // Sử dụng Bounds của Collider sẽ chính xác hơn dùng Scale
        float halfHeight = solidCollider.bounds.extents.y;

        // 2. Điểm bắt đầu bắn tia: Từ tâm của vật thể
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f; // Nhích lên một chút để tránh bắn xuyên sàn

        // 3. Khoảng cách check: Chỉ dài hơn một chút so với khoảng cách từ tâm đến đáy
        // Ví dụ: nửa chiều cao là 1m, ta check 1.1m hoặc 1.2m
        float extraBuffer = 0.2f;
        float checkDistance = halfHeight + extraBuffer;

        RaycastHit hit;
        // Vẽ Ray trong Scene để bạn dễ debug (chỉ thấy trong cửa sổ Scene)
        Debug.DrawRay(rayOrigin, Vector3.down * checkDistance, Color.yellow);

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, checkDistance, groundLayer))
        {
            isGrounded = true;

            // Tính toán xoay theo mặt phẳng
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
        else
        {
            isGrounded = false;
            // Reset về tư thế đứng thẳng khi lơ lửng
            Quaternion uprightRotation = Quaternion.FromToRotation(transform.up, Vector3.up) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, uprightRotation, rotateSpeed * Time.deltaTime);
        }
    }

    #region || --- On Triggers (Chỉ check va chạm vật cản) --- |

    // Lưu ý: Dùng Trigger để check Ground là không tốt, chỉ nên dùng để check vật cản (Cây, đá, nhà khác)
    private void OnTriggerEnter(Collider other)
    {
        // Bỏ qua Ground trong trigger vì đã xử lý bằng Raycast trong Update chính xác hơn
        if (other.CompareTag("Ground") || ((1 << other.gameObject.layer) & groundLayer) != 0) return;

        if (other.CompareTag("Tree") || other.CompareTag("pickable") || other.CompareTag("Building"))
        {
            overlapCount++; // Tăng biến đếm
            isOverlappingItems = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground") || ((1 << other.gameObject.layer) & groundLayer) != 0) return;

        if (other.CompareTag("Tree") || other.CompareTag("pickable") || other.CompareTag("Building"))
        {
            overlapCount--; // Giảm biến đếm
            if (overlapCount <= 0)
            {
                overlapCount = 0;
                isOverlappingItems = false;
            }
        }
    }
    #endregion

    #region || --- Set Outline Colors --- |
    public void SetInvalidColor()
    {
        if (outline != null)
        {
            outline.enabled = true;
            outline.OutlineColor = Color.red;
            outline.OutlineWidth = 5f; // Đảm bảo nhìn thấy rõ
        }
    }

    public void SetValidColor()
    {
        if (outline != null)
        {
            outline.enabled = true;
            outline.OutlineColor = Color.green;
            outline.OutlineWidth = 3f;
        }
    }

    public void SetDefaultColor()
    {
        if (outline != null)
        {
            outline.enabled = false;
        }
    }
    #endregion
}