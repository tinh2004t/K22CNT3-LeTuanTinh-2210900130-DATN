using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LoadSlot : MonoBehaviour
{
    public Button button;
    public TMPro.TextMeshProUGUI buttonText;

    public int slotNumber;

    

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonText = transform.Find("Text (TMP)").GetComponent<TMPro.TextMeshProUGUI>();

        
    }

    private void Update()
    {
        if (SaveManager.Instance.isSlotEmpty(slotNumber))
        {
            buttonText.text = "";
        }
        else
        {
            buttonText.text = PlayerPrefs.GetString("Slot" + slotNumber + "Description");
        }
    }

    

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            if (!SaveManager.Instance.isSlotEmpty(slotNumber))
            {
                SaveManager.Instance.StartLoadedGame(slotNumber);
                SaveManager.Instance.DeselectButton();
            }
            else
            {
                // DisplayOverWarning
            }
        });
    }
}
