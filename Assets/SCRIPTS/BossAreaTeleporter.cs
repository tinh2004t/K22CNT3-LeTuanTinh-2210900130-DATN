using UnityEngine;

public class BossAreaTeleporter : MonoBehaviour
{
    [Header("Cấu hình dịch chuyển")]
    public Transform destination; // Kéo BossArena_SpawnPoint vào đây
    public GameObject vfxEffect; // Hiệu ứng biến mất (tùy chọn)

    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra nếu vật thể chạm vào là Player
        if (other.CompareTag("Player"))
        {
            TeleportPlayer(other.gameObject);
        }
    }

    void TeleportPlayer(GameObject player)
    {
        // Nếu người chơi dùng CharacterController, phải tắt nó đi trước khi đổi vị trí
        CharacterController cc = player.GetComponent<CharacterController>();

        if (cc != null) cc.enabled = false;

        // Dịch chuyển vị trí và góc quay
        player.transform.position = destination.position;
        player.transform.rotation = destination.rotation;

        if (cc != null) cc.enabled = true;

        // Tạo hiệu ứng tại điểm đến nếu có
        if (vfxEffect != null)
        {
            Instantiate(vfxEffect, destination.position, Quaternion.identity);
        }

        Debug.Log("Đã đưa người chơi vào đấu trường Boss!");
    }
}