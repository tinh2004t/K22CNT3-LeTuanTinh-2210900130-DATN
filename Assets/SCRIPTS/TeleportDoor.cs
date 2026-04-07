using UnityEngine;

public class TeleportDoor : MonoBehaviour
{
    [Header("Điểm đến")]
    [Tooltip("Kéo thả GameObject đích đến vào đây")]
    public Transform destination;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TeleportPlayer(other.gameObject);
        }
    }

    private void TeleportPlayer(GameObject player)
    {
        CharacterController controller = player.GetComponent<CharacterController>();

        if (controller != null)
        {
            controller.enabled = false;
            player.transform.position = destination.position;
            controller.enabled = true;
        }
        else
        {
            player.transform.position = destination.position;
        }

        Debug.Log("Đã dịch chuyển người chơi!");
    }
}