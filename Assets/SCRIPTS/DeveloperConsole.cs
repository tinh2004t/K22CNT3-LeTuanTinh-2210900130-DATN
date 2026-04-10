using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeveloperConsole : MonoBehaviour
{
    public static DeveloperConsole Instance { get; set; }

    [Header("UI References")]
    public GameObject consoleUI;
    public TMP_Dropdown commandDropdown;
    public TMP_InputField commandInput;

    private bool isConsoleOpen = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    private void Start()
    {
        consoleUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F9))
        {
            ToggleConsole();
        }

        if (isConsoleOpen && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            ExecuteCommand();
        }
    }

    public void ToggleConsole()
    {
        isConsoleOpen = !isConsoleOpen;
        consoleUI.SetActive(isConsoleOpen);

        if (isConsoleOpen)
        {
            MovementManager.Instance.EnableLook(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            commandInput.ActivateInputField();
            commandInput.text = "";
        }
        else
        {
            MovementManager.Instance.EnableLook(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void ExecuteCommand()
    {
        if (string.IsNullOrWhiteSpace(commandInput.text)) return;

        string commandType = commandDropdown.options[commandDropdown.value].text.ToLower();

        string[] arguments = commandInput.text.Trim().Split(' ');

        switch (commandType)
        {
            case "give":
                HandleGiveCommand(arguments);
                break;
            case "tele":
                HandleTeleportCommand(arguments);
                break;
            case "time":
                HandleTimeCommand(arguments);
                break;
            default:
                Debug.LogWarning("Không tìm thấy lệnh này!");
                break;
        }

        commandInput.text = "";
        commandInput.ActivateInputField();
    }

    private void HandleGiveCommand(string[] args)
    {
        if (args.Length < 1)
        {
            Debug.LogWarning("Sai cú pháp! Ví dụ: bread 2");
            return;
        }

        string itemName = args[0];
        int amount = 1; 

        if (args.Length >= 2)
        {
            if (!int.TryParse(args[1], out amount))
            {
                Debug.LogWarning("Số lượng không hợp lệ!");
                return;
            }
        }

        if (InventorySystem.Instance.CheckSlotsAvailable(amount))
        {
            for (int i = 0; i < amount; i++)
            {
                InventorySystem.Instance.AddToInventory(itemName);
            }
            Debug.Log($"Đã thêm {amount} {itemName} vào túi đồ.");
        }
        else
        {
            Debug.LogWarning("Túi đồ không đủ chỗ trống để chứa lệnh này!");
        }
    }

    private void HandleTeleportCommand(string[] args)
    {
        if (args.Length < 1)
        {
            Debug.LogWarning("Sai cú pháp! Ví dụ: tele Dana");
            return;
        }

        string targetName = args[0];

        GameObject target = GameObject.Find(targetName);

        if (target == null)
        {
            Debug.LogWarning($"Không tìm thấy đối tượng nào tên là '{targetName}' trong scene!");
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            CharacterController cc = player.GetComponent<CharacterController>();

            if (cc != null) cc.enabled = false;

            player.transform.position = target.transform.position + target.transform.forward * 1.5f + Vector3.up * 0.5f;

            if (cc != null) cc.enabled = true;

            Debug.Log($"Đã dịch chuyển tới {targetName}.");
        }
        else
        {
            Debug.LogWarning("Không tìm thấy GameObject nào có tag 'Player'!");
        }
    }

    private void HandleTimeCommand(string[] args)
    {
        if (args.Length < 1)
        {
            Debug.LogWarning("Sai cú pháp! Dùng: time scale | time day | time night | time set | time speed");
            return;
        }

        string subCommand = args[0].ToLower();

        switch (subCommand)
        {
            case "scale":
                if (args.Length >= 2 && float.TryParse(args[1], out float scaleValue))
                {
                    Time.timeScale = scaleValue;
                    Debug.Log($"Đã chỉnh tốc độ game (Time.timeScale) thành: {scaleValue}");
                }
                else
                {
                    Debug.LogWarning("Vui lòng nhập số. Ví dụ: time scale 1.5");
                }
                break;

            case "day":
                if (DayNightSystem.Instance != null)
                {
                    DayNightSystem.Instance.currentTimeOfDay = 0.5f;
                    Debug.Log("Đã chuyển thời gian thành Ban Ngày (12:00).");
                }
                break;

            case "night":
                if (DayNightSystem.Instance != null)
                {
                    DayNightSystem.Instance.currentTimeOfDay = 0f;
                    Debug.Log("Đã chuyển thời gian thành Ban Đêm (00:00).");
                }
                break;

            case "set":
                if (args.Length >= 2 && float.TryParse(args[1], out float targetHour))
                {
                    if (targetHour >= 0 && targetHour <= 24)
                    {
                        if (DayNightSystem.Instance != null)
                        {
                            DayNightSystem.Instance.currentTimeOfDay = targetHour / 24f;
                            Debug.Log($"Đã chỉnh thời gian thành: {targetHour}:00");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Giờ phải nằm trong khoảng từ 0 đến 24!");
                    }
                }
                else
                {
                    Debug.LogWarning("Vui lòng nhập giờ hợp lệ. Ví dụ: time set 15");
                }
                break;

            case "speed":
                if (args.Length >= 2 && float.TryParse(args[1], out float duration))
                {
                    if (DayNightSystem.Instance != null)
                    {
                        DayNightSystem.Instance.dayDurationInSeconds = duration;
                        Debug.Log($"Đã chỉnh độ dài một ngày (Day Duration) thành: {duration} giây");
                    }
                }
                else
                {
                    Debug.LogWarning("Vui lòng nhập số giây. Ví dụ: time speed 24");
                }
                break;

            default:
                Debug.LogWarning("Lệnh không hợp lệ! Dùng: time scale, time day, time night, time set, time speed.");
                break;
        }
    }
}