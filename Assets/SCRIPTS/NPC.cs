using System;
using UnityEngine;
using TMPro;

public class NPC : MonoBehaviour
{

    public bool playerInRange;

    public bool isTalkingWithPlayer;

    internal void StartConversation()
    {
        isTalkingWithPlayer = true;

        print("NPC: Hello, traveler! Welcome to our village.");

        DialogSystem.Instance.OpenDialogUI();
        DialogSystem.Instance.dialogText.text = "NPC: Hello, traveler! Welcome to our village.";
        DialogSystem.Instance.option1BTN.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "1. Bye!!";
        DialogSystem.Instance.option1BTN.onClick.AddListener(() =>
        {
            DialogSystem.Instance.CloseDialogUI();
            isTalkingWithPlayer = false;

        });

    }

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
}
