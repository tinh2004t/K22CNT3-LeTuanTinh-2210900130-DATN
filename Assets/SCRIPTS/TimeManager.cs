using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; set; }

    public int dayInGame = 1;

    public TextMeshProUGUI dayUI;

    private void Awake()
    {
        // Khởi tạo Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        UpdateDayUI();
    }

    public void TriggerNextDay()
    {
        dayInGame += 1; // Tăng ngày lên 1
        UpdateDayUI();
    }

    void UpdateDayUI()
    {
        dayUI.text = "Day: " + dayInGame;
    }
}