using System;
using UnityEngine;
using UnityEngine.UI;

public class ShopKeeper : MonoBehaviour
{
    public bool playerInRange;
    public bool isTalkingWithPlayer;

    public GameObject shopkeeperDialogUI;
    public Button buyBTN;
    public Button sellBTN;
    public Button exitBTN;

    public GameObject buyPanelUI;
    public GameObject sellPanelUI;

    private void Start()
    {
        shopkeeperDialogUI.SetActive(false);
        buyBTN.onClick.AddListener(BuyMode);
        sellBTN.onClick.AddListener(SellMode);
        exitBTN.onClick.AddListener(StopTalking);
    }

    private void SellMode()
    {
        sellPanelUI.SetActive(true);
        buyPanelUI.SetActive(false);

        HideDialogUI();
    }

    private void BuyMode()
    {
        sellPanelUI.SetActive(false);
        buyPanelUI.SetActive(true);

        HideDialogUI();
    }

    public void DialogMode()
    {
        DisplayDialogUI();

        sellPanelUI.SetActive(false);
        buyPanelUI.SetActive(false);

        
    }

    public void Talk()
    {
        isTalkingWithPlayer = true;
        DisplayDialogUI();

        MovementManager.Instance.EnableLook(false);
        MovementManager.Instance.EnableMovement(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    public void StopTalking()
    {
        isTalkingWithPlayer = false;
        HideDialogUI();

        MovementManager.Instance.EnableLook(true);
        MovementManager.Instance.EnableMovement(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void DisplayDialogUI()
    {
        shopkeeperDialogUI.SetActive(true);
    }

    private void HideDialogUI()
    {
        shopkeeperDialogUI.SetActive(false);
    }

    #region || ----- Ontrigger Methods ----- ||
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
    #endregion
}
