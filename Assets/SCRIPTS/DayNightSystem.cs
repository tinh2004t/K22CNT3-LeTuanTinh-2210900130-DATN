using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayNightSystem : MonoBehaviour
{
    public static DayNightSystem Instance;

    [Header("Settings")]
    public Light directionalLight;
    public float dayDurationInSeconds = 24.0f;

    [Header("Status")]
    [Range(0, 1)]
    public float currentTimeOfDay = 0.35f;
    public int currentHour;

    [Header("UI")]
    public TextMeshProUGUI timeUI;

    [Header("Skybox & Environment")]
    public List<SkyboxTimeMapping> timeMappings;

    private float blendedValue = 0.0f;
    private bool lockNextDayTrigger = false;
    private int lastHour = -1;

    // Cache lại mapping hiện tại để không phải loop mỗi frame
    private SkyboxTimeMapping currentMapping;
    // Cache lại ID của thuộc tính shader để truy xuất siêu nhanh thay vì dùng string
    private int transitionFactorID;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        // Tối ưu: Lấy ID của property thay vì dùng string tên
        transitionFactorID = Shader.PropertyToID("_TransitionFactor");
    }

    public bool IsNight()
    {
        return currentHour < 6 || currentHour >= 18;
    }

    private void Update()
    {
        // 1. Tăng thời gian
        currentTimeOfDay += Time.deltaTime / dayDurationInSeconds;
        currentTimeOfDay %= 1;

        // 2. Tính toán giờ
        currentHour = Mathf.FloorToInt(currentTimeOfDay * 24);

        // 3. Xử lý khi GIỜ THAY ĐỔI (Chỉ chạy 1 lần mỗi giờ in-game)
        if (currentHour != lastHour)
        {
            OnHourChanged();
            lastHour = currentHour;
        }

        // 4. Cập nhật UI
        if (timeUI != null)
        {
            timeUI.text = $"{currentHour:00}:00";
        }

        // 5. Xoay mặt trời
        if (directionalLight != null)
        {
            directionalLight.transform.rotation = Quaternion.Euler(new Vector3((currentTimeOfDay * 360f) - 90f, 170f, 0));
        }

        // 6. Xử lý Blend Skybox mượt mà mỗi frame
        HandleSkyboxBlending();
    }

    private void OnHourChanged()
    {
        // Kích hoạt qua ngày mới
        if (currentHour == 0 && !lockNextDayTrigger)
        {
            if (TimeManager.Instance != null) TimeManager.Instance.TriggerNextDay();
            lockNextDayTrigger = true;
        }
        else if (currentHour != 0)
        {
            lockNextDayTrigger = false;
        }

        // Tìm Skybox phù hợp cho giờ hiện tại (Chỉ loop 1 lần/giờ)
        foreach (SkyboxTimeMapping mapping in timeMappings)
        {
            if (currentHour == mapping.hour)
            {
                currentMapping = mapping;
                blendedValue = 0f; // Reset blend value cho skybox mới

                if (RenderSettings.skybox != currentMapping.skyboxMaterial)
                {
                    RenderSettings.skybox = currentMapping.skyboxMaterial;
                    DynamicGI.UpdateEnvironment();
                }
                break;
            }
        }
    }

    private void HandleSkyboxBlending()
    {
        // Kiểm tra xem mapping hiện tại có dùng shader blend không
        if (currentMapping != null && RenderSettings.skybox != null)
        {
            // Tối ưu hiệu năng: Nếu có thay đổi thông số thì gọi SetFloat bằng ID
            if (RenderSettings.skybox.HasProperty(transitionFactorID) && blendedValue < 1f)
            {
                blendedValue += Time.deltaTime; // Bạn có thể chia cho 1 biến duration để chỉnh tốc độ blend
                blendedValue = Mathf.Clamp01(blendedValue);

                // Chỉnh sửa trực tiếp trên RenderSettings.skybox (an toàn hơn là sửa trên file asset mapping)
                RenderSettings.skybox.SetFloat(transitionFactorID, blendedValue);
            }
        }
    }
}

[System.Serializable]
public class SkyboxTimeMapping
{
    public string phaseName;
    public int hour;
    public Material skyboxMaterial;
}