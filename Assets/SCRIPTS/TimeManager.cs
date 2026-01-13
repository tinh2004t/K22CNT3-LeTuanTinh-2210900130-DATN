using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; set; }

    public UnityEvent OnDayPass = new UnityEvent();

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

        OnDayPass.Invoke(); // Gọi sự kiện ngày mới
    }

    void UpdateDayUI()
    {
        dayUI.text = "Day: " + dayInGame;
    }
}