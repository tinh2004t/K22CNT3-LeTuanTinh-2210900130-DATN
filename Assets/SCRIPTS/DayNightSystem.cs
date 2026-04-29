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

    private SkyboxTimeMapping currentMapping;
    private int transitionFactorID;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        transitionFactorID = Shader.PropertyToID("_TransitionFactor");
    }

    public bool IsNight()
    {
        return currentHour < 6 || currentHour >= 18;
    }

    private void Update()
    {
        currentTimeOfDay += Time.deltaTime / dayDurationInSeconds;
        currentTimeOfDay %= 1;

        currentHour = Mathf.FloorToInt(currentTimeOfDay * 24);

        if (currentHour != lastHour)
        {
            OnHourChanged();
            lastHour = currentHour;
        }

        if (timeUI != null)
        {
            timeUI.text = $"{currentHour:00}:00";
        }

        if (directionalLight != null)
        {
            directionalLight.transform.rotation = Quaternion.Euler(new Vector3((currentTimeOfDay * 360f) - 90f, 170f, 0));
        }

        HandleSkyboxBlending();
    }

    private void OnHourChanged()
    {
        if (currentHour == 0 && !lockNextDayTrigger)
        {
            if (TimeManager.Instance != null) TimeManager.Instance.TriggerNextDay();
            lockNextDayTrigger = true;
        }
        else if (currentHour != 0)
        {
            lockNextDayTrigger = false;
        }

        foreach (SkyboxTimeMapping mapping in timeMappings)
        {
            if (currentHour == mapping.hour)
            {
                currentMapping = mapping;
                blendedValue = 0f; 

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
        if (currentMapping != null && RenderSettings.skybox != null)
        {
            if (RenderSettings.skybox.HasProperty(transitionFactorID) && blendedValue < 1f)
            {
                blendedValue += Time.deltaTime; 
                blendedValue = Mathf.Clamp01(blendedValue);

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