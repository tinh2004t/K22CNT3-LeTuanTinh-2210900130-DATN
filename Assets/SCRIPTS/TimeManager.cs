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
        dayInGame += 1; 
        UpdateDayUI();

        OnDayPass.Invoke(); 
    }

    void UpdateDayUI()
    {
        dayUI.text = "Day: " + dayInGame;
    }
}