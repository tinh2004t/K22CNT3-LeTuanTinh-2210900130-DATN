using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SaveManager;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get;  set; }

    public Button backBTN;

    public Slider masterSlider;
    public GameObject masterValue;

    public Slider musicSlider;
    public GameObject musicValue;

    public Slider effectsSlider;
    public GameObject effectsValue;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        backBTN.onClick.AddListener(() => {
            SaveManager.Instance.SaveVolumeSettings(musicSlider.value, effectsSlider.value, masterSlider.value);
        });

        StartCoroutine(LoadApplySettings());

    }

    private IEnumerator LoadApplySettings()
    {
        LoadAndSetVolume();

        //Load Graphics Settings
        //Key Bindings

        yield return new WaitForSeconds(0.1f);
    }

    private void LoadAndSetVolume()
    {
        VolumeSettings volumeSettings = SaveManager.Instance.LoadVolumeSettings();

        musicSlider.value = volumeSettings.music;
        effectsSlider.value = volumeSettings.effects;
        masterSlider.value = volumeSettings.master;

        print("LOADED VOLUME SETTINGS: " + JsonUtility.ToJson(volumeSettings));
    }

    private void Update()
    {
        masterValue.GetComponent<TextMeshProUGUI>().text = "" + (masterSlider.value) + "";
        musicValue.GetComponent<TextMeshProUGUI>().text = "" + (musicSlider.value) + "";
        effectsValue.GetComponent<TextMeshProUGUI>().text = "" + (effectsSlider.value) + "";
    }


}
