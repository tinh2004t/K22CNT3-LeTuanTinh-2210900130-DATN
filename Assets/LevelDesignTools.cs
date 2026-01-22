using UnityEngine;
using UnityEditor;

public class LevelDesignTools : EditorWindow
{
    [MenuItem("Tools/Drop To Ground %g")] // Ctrl + G
    public static void DropToGround()
    {
        // 1. Tạo LayerMask để Raycast CHỈ trúng những vật có Layer là "Terrain" (hoặc Default nếu bạn chưa set)
        // Tuy nhiên, cách an toàn nhất cho trường hợp của bạn (đang dùng Tag) là dùng RaycastAll

        int successCount = 0;

        foreach (GameObject obj in Selection.gameObjects)
        {
            // Tạm thời tắt Collider của chính vật thể đó để tránh bắn vào chân mình
            Collider objCollider = obj.GetComponent<Collider>();
            bool wasEnabled = false;
            if (objCollider != null)
            {
                wasEnabled = objCollider.enabled;
                objCollider.enabled = false; // Tắt collider đi
            }

            // Bắn tia Raycast từ độ cao rất lớn (500 đơn vị)
            RaycastHit[] hits = Physics.RaycastAll(obj.transform.position + Vector3.up * 500f, Vector3.down, 1000f);

            // Tìm trong tất cả các vật va chạm, cái nào là "Ground"
            foreach (RaycastHit hit in hits)
            {
                // Kiểm tra Tag "Ground" như bạn yêu cầu
                if (hit.collider.CompareTag("Ground") || hit.collider.gameObject.name.Contains("Terrain"))
                {
                    // Lưu lại rotation cũ nếu muốn giữ nguyên góc xoay
                    // Quaternion oldRot = obj.transform.rotation;

                    // Đặt vị trí
                    obj.transform.position = hit.point;

                    // (Tùy chọn) Xoay theo độ dốc địa hình. Nếu cây bị nghiêng quá thì comment dòng dưới lại.
                    // obj.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * obj.transform.rotation; 

                    successCount++;
                    break; // Đã tìm thấy đất, thoát vòng lặp hits
                }
            }

            // Bật lại Collider cho vật thể
            if (objCollider != null)
            {
                objCollider.enabled = wasEnabled;
            }
        }

        Debug.Log($"Đã hạ cánh thành công {successCount} / {Selection.gameObjects.Length} vật thể xuống 'Ground'!");
    }
}