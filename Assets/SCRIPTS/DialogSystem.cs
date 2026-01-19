using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{

    public static DialogSystem Instance { get; set; }

    public TextMeshProUGUI dialogText;


    public Button option1BTN;
    public Button option2BTN;

    public Canvas dialogUI;

    public bool dialogUIActive;

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

    public void OpenDialogUI()
    {
        dialogUI.gameObject.SetActive(true);
        dialogUIActive = true;

        MovementManager.Instance.EnableLook(false);
        MovementManager.Instance.EnableMovement(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseDialogUI()
    {
        dialogUI.gameObject.SetActive(false);
        dialogUIActive = false;

        MovementManager.Instance.EnableLook(true);
        MovementManager.Instance.EnableMovement(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}
