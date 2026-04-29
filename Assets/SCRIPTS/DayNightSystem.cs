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

<<<<<<< HEAD
    private SkyboxTimeMapping currentMapping;
=======
    // Cache lại mapping hiện tại để không phải loop mỗi frame
    private SkyboxTimeMapping currentMapping;
    // Cache lại ID của thuộc tính shader để truy xuất siêu nhanh thay vì dùng string
>>>>>>> origin/main
    private int transitionFactorID;

    private void Awake()
    {
        if (Instance == null) Instance = this;
<<<<<<< HEAD
=======
        // Tối ưu: Lấy ID của property thay vì dùng string tên
>>>>>>> origin/main
        transitionFactorID = Shader.PropertyToID("_TransitionFactor");
    }

    public bool IsNight()
    {
        return currentHour < 6 || currentHour >= 18;
    }

    private void Update()
    {
<<<<<<< HEAD
        currentTimeOfDay += Time.deltaTime / dayDurationInSeconds;
        currentTimeOfDay %= 1;

        currentHour = Mathf.FloorToInt(currentTimeOfDay * 24);

=======
        // 1. Tăng thời gian
        currentTimeOfDay += Time.deltaTime / dayDurationInSeconds;
        currentTimeOfDay %= 1;

        // 2. Tính toán giờ
        currentHour = Mathf.FloorToInt(currentTimeOfDay * 24);

        // 3. Xử lý khi GIỜ THAY ĐỔI (Chỉ chạy 1 lần mỗi giờ in-game)
>>>>>>> origin/main
        if (currentHour != lastHour)
        {
            OnHourChanged();
            lastHour = currentHour;
        }

<<<<<<< HEAD
=======
        // 4. Cập nhật UI
>>>>>>> origin/main
        if (timeUI != null)
        {
            timeUI.text = $"{currentHour:00}:00";
        }

<<<<<<< HEAD
=======
        // 5. Xoay mặt trời
>>>>>>> origin/main
        if (directionalLight != null)
        {
            directionalLight.transform.rotation = Quaternion.Euler(new Vector3((currentTimeOfDay * 360f) - 90f, 170f, 0));
        }

<<<<<<< HEAD
=======
        // 6. Xử lý Blend Skybox mượt mà mỗi frame
>>>>>>> origin/main
        HandleSkyboxBlending();
    }

    private void OnHourChanged()
    {
<<<<<<< HEAD
=======
        // Kích hoạt qua ngày mới
>>>>>>> origin/main
        if (currentHour == 0 && !lockNextDayTrigger)
        {
            if (TimeManager.Instance != null) TimeManager.Instance.TriggerNextDay();
            lockNextDayTrigger = true;
        }
        else if (currentHour != 0)
        {
            lockNextDayTrigger = false;
        }

<<<<<<< HEAD
=======
        // Tìm Skybox phù hợp cho giờ hiện tại (Chỉ loop 1 lần/giờ)
>>>>>>> origin/main
        foreach (SkyboxTimeMapping mapping in timeMappings)
        {
            if (currentHour == mapping.hour)
            {
                currentMapping = mapping;
<<<<<<< HEAD
                blendedValue = 0f; 
=======
                blendedValue = 0f; // Reset blend value cho skybox mới
>>>>>>> origin/main

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
<<<<<<< HEAD
        if (currentMapping != null && RenderSettings.skybox != null)
        {
            if (RenderSettings.skybox.HasProperty(transitionFactorID) && blendedValue < 1f)
            {
                blendedValue += Time.deltaTime; 
                blendedValue = Mathf.Clamp01(blendedValue);

=======
        // Kiểm tra xem mapping hiện tại có dùng shader blend không
        if (currentMapping != null && RenderSettings.skybox != null)
        {
            // Tối ưu hiệu năng: Nếu có thay đổi thông số thì gọi SetFloat bằng ID
            if (RenderSettings.skybox.HasProperty(transitionFactorID) && blendedValue < 1f)
            {
                blendedValue += Time.deltaTime; // Bạn có thể chia cho 1 biến duration để chỉnh tốc độ blend
                blendedValue = Mathf.Clamp01(blendedValue);

                // Chỉnh sửa trực tiếp trên RenderSettings.skybox (an toàn hơn là sửa trên file asset mapping)
>>>>>>> origin/main
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