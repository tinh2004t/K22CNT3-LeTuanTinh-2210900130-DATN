using System.Collections.Generic;
using UnityEngine;

public class BlacksmithNPC : MonoBehaviour
{
    public bool playerInRange;
    public bool isTalkingWithPlayer;

    public UpgradeRecipeSO npcRecipe; // Công thức của NPC này


    [Header("Danh sách rèn")]
    public List<UpgradeRecipeSO> npcRecipes; // Đổi thành List

    public void Talk()
    {
        isTalkingWithPlayer = true;
        LookAtPlayer();

        // Truyền List vào
        UpgradeUIManager.Instance.OpenMenu(npcRecipes);

        MovementManager.Instance.EnableLook(false);
        MovementManager.Instance.EnableMovement(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StopTalking()
    {
        isTalkingWithPlayer = false;

        // 1. Đóng giao diện nâng cấp
        UpgradeUIManager.Instance.CloseMenu();

        // 2. Trả lại quyền điều khiển cho Player
        MovementManager.Instance.EnableLook(true);
        MovementManager.Instance.EnableMovement(true);

        // 3. Ẩn chuột đi
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void LookAtPlayer()
    {
        var player = PlayerState.Instance.playerBody.transform;
        Vector3 direction = player.position - transform.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (isTalkingWithPlayer)
            {
                StopTalking(); // Tự động đóng nếu lỡ bị đẩy ra xa
            }
        }
    }
}