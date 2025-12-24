using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    private Button button;
    private TMPro.TextMeshProUGUI buttonText;

    public int slotNumber;

    public GameObject alertUI;
    Button yesBTN;
    Button noBTN;

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonText = transform.Find("Text (TMP)").GetComponent<TMPro.TextMeshProUGUI>();

        yesBTN = alertUI.transform.Find("YesButton").GetComponent<Button>();
        noBTN = alertUI.transform.Find("NoButton").GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            if (SaveManager.Instance.isSlotEmpty(slotNumber))
            {
                SaveGameComfirmed();
            }
            else
            {
                DisplayOverrideAlert();
            }
        });
    }

    private void Update()
    {
        if (SaveManager.Instance.isSlotEmpty(slotNumber))
        {
            buttonText.text = "Empty";
        }
        else
        {
            buttonText.text = PlayerPrefs.GetString("Slot" + slotNumber + "Description");
        }
    }

    public void DisplayOverrideAlert()
    {
        alertUI.SetActive(true);

        yesBTN.onClick.AddListener(() =>
        {
            SaveGameComfirmed();
            alertUI.SetActive(false);
        });
        noBTN.onClick.AddListener(() =>
        {
            alertUI.SetActive(false);
        });
    }

    private void SaveGameComfirmed()
    {
        SaveManager.Instance.SaveGame(slotNumber);

        DateTime dt = DateTime.Now;
        string time = dt.ToString("yyyy-MM-dd HH:mm");
        string description = "Saved Game " + slotNumber + " | " + time;

        buttonText.text = description;

        PlayerPrefs.SetString("Slot" + slotNumber + "Description", description);

        SaveManager.Instance.DeselectButton();
    }
}
