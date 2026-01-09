using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayNightSystem : MonoBehaviour
{
    [Header("Settings")]
    public Light directionalLight;
    public float dayDurationInSeconds = 24.0f;

    [Header("Status")]
    [Range(0, 1)]
    public float currentTimeOfDay = 0.35f; // Giá trị chạy từ 0 -> 1
    public int currentHour; // Giá trị chạy từ 0 -> 23

    [Header("UI")]
    public TextMeshProUGUI timeUI;

    [Header("Skybox & Environment")]
    public List<SkyboxTimeMapping> timeMappings;

    private float blendedValue = 0.0f;
    private bool lockNextDayTrigger = false;
    private int lastHour = -1; // Dùng để kiểm tra xem giờ có thay đổi không

    private void Update()
    {
        // 1. Tăng thời gian
        currentTimeOfDay += Time.deltaTime / dayDurationInSeconds;
        currentTimeOfDay %= 1; // Giữ giá trị luôn trong khoảng 0 đến 1

        // 2. Tính toán giờ hiện tại (SỬA LỖI: Gán vào currentHour, không gán vào currentTimeOfDay)
        currentHour = Mathf.FloorToInt(currentTimeOfDay * 24);

        // 3. Cập nhật UI
        if (timeUI != null)
        {
            // Format chuỗi đẹp hơn (VD: 8:00 thay vì 8:00)
            timeUI.text = $"{currentHour:00}:00";
        }

        // 4. Xoay mặt trời
        // (currentTimeOfDay * 360) - 90: Để 0h (nửa đêm) ánh sáng hướng từ dưới lên, 6h sáng mặt trời mọc, 12h trưa đỉnh đầu
        if (directionalLight != null)
        {
            directionalLight.transform.rotation = Quaternion.Euler(new Vector3((currentTimeOfDay * 360f) - 90f, 170f, 0));
        }

        // 5. Xử lý Skybox
        UpdateSkybox();

        // 6. Xử lý sự kiện qua ngày mới
        CheckNextDayEvent();
    }

    void UpdateSkybox()
    {
        // Tối ưu: Chỉ tìm Skybox khi giờ thay đổi, hoặc nếu bạn cần blend liên tục thì phải cẩn thận
        // Ở đây tôi giữ logic tìm skybox theo giờ của bạn

        Material targetSkybox = null;

        foreach (SkyboxTimeMapping mapping in timeMappings)
        {
            if (currentHour == mapping.hour)
            {
                targetSkybox = mapping.skyboxMaterial;

                // Logic Blend Shader của bạn
                if (targetSkybox != null && targetSkybox.shader != null)
                {
                    if (targetSkybox.shader.name == "Custom/SkyboxTransition")
                    {
                        blendedValue += Time.deltaTime;
                        blendedValue = Mathf.Clamp01(blendedValue);
                        targetSkybox.SetFloat("_TransitionFactor", blendedValue);
                    }
                    else
                    {
                        blendedValue = 0; // Reset nếu không phải shader transition
                    }
                }
                break;
            }
        }

        // Chỉ set Skybox nếu tìm thấy và skybox hiện tại khác với cái mới (để tối ưu)
        if (targetSkybox != null && RenderSettings.skybox != targetSkybox)
        {
            RenderSettings.skybox = targetSkybox;
            // Cập nhật lại ánh sáng môi trường khi đổi skybox (quan trọng cho GI)
            DynamicGI.UpdateEnvironment();
        }
    }

    void CheckNextDayEvent()
    {
        if (currentHour == 0 && !lockNextDayTrigger)
        {
            if (TimeManager.Instance != null)
            {
                TimeManager.Instance.TriggerNextDay();
                Debug.Log("New Day Triggered!");
            }
            lockNextDayTrigger = true;
        }

        if (currentHour != 0)
        {
            lockNextDayTrigger = false;
        }
    }
}

[System.Serializable]
public class SkyboxTimeMapping
{
    public string phaseName; // VD: Morning, Noon, Sunset, Night
    public int hour;         // VD: 6, 12, 18, 22
    public Material skyboxMaterial;
}