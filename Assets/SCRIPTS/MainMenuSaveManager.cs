using UnityEngine;
using UnityEngine.Rendering;

public class MainMenuSaveManager : MonoBehaviour
{

    public static MainMenuSaveManager Instance { get;  set; }
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


    // --- Volume Setting --- //
    [System.Serializable]
    public class VolumeSettings
    {
        public float music;
        public float effects;
        public float master;
    }

    public void SaveVolumeSettings(float _music, float _effects, float _master)
    {
        VolumeSettings volumeSettings = new VolumeSettings
        {
            music = _music,
            effects = _effects,
            master = _master
        };

        print("SAVING VOLUME SETTINGS: " + JsonUtility.ToJson(volumeSettings));

        PlayerPrefs.SetString("VolumeSettings", JsonUtility.ToJson(volumeSettings));
        PlayerPrefs.Save();

    }

    public VolumeSettings LoadVolumeSettings()
    {
        return JsonUtility.FromJson<VolumeSettings>(PlayerPrefs.GetString("VolumeSettings"));
        
    }

}
