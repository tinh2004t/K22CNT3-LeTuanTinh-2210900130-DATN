using UnityEngine;
using UnityEditor;

public class LevelDesignTools : EditorWindow
{
    [MenuItem("Tools/Drop To Ground %g")] 
    public static void DropToGround()
    {

        int successCount = 0;

        foreach (GameObject obj in Selection.gameObjects)
        {
            Collider objCollider = obj.GetComponent<Collider>();
            bool wasEnabled = false;
            if (objCollider != null)
            {
                wasEnabled = objCollider.enabled;
                objCollider.enabled = false; 
            }

            RaycastHit[] hits = Physics.RaycastAll(obj.transform.position + Vector3.up * 500f, Vector3.down, 1000f);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag("Ground") || hit.collider.gameObject.name.Contains("Terrain"))
                {
                    obj.transform.position = hit.point;

                    successCount++;
                    break; 
                }
            }

            if (objCollider != null)
            {
                objCollider.enabled = wasEnabled;
            }
        }

        Debug.Log($"Đã hạ cánh thành công {successCount} / {Selection.gameObjects.Length} vật thể xuống 'Ground'!");
    }
}