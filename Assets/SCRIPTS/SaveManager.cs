using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{

    public static SaveManager Instance { get;  set; }
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

        DontDestroyOnLoad(gameObject);
    }

    //Json Project Save Path
    public string jsonPathProject;

    // Json External/ Real Save Path
    public string jsonPathPersistant;

    // Binary Save Path
    string binaryPath;


    public bool isSavingToJson;

    private void Start()
    {
        jsonPathProject = Application.dataPath + Path.AltDirectorySeparatorChar + "SaveGame.json";
        jsonPathPersistant = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveGame.json";
        binaryPath = Application.persistentDataPath + "/save_game.bin";
    }


    #region || ----- General Section ----- ||
    #region || ----- Saving ----- ||

    public void SaveGame()
    {
        AllGameData data = new AllGameData();
        data.playerData = GetPlayerData();

        SavingTypeSwitch(data);


    }

    private PlayerData GetPlayerData()
    {
        float[] playerStats = new float[3];
        playerStats[0] = PlayerState.Instance.currentHealth;
        playerStats[1] = PlayerState.Instance.currentCalories;
        playerStats[2] = PlayerState.Instance.currentHydrationPercent;


        float[] playerPosAndRot = new float[6];
        playerPosAndRot[0] = PlayerState.Instance.playerBody.transform.position.x;
        playerPosAndRot[1] = PlayerState.Instance.playerBody.transform.position.y;
        playerPosAndRot[2] = PlayerState.Instance.playerBody.transform.position.z;

        playerPosAndRot[3] = PlayerState.Instance.playerBody.transform.rotation.x;
        playerPosAndRot[4] = PlayerState.Instance.playerBody.transform.rotation.y;
        playerPosAndRot[5] = PlayerState.Instance.playerBody.transform.rotation.z;

        return new PlayerData(playerStats, playerPosAndRot);

    }

    public void SavingTypeSwitch(AllGameData gameData)
    {
        if (isSavingToJson)
        {
            SaveGameDataToJsonFile(gameData);

        }
        else
        {
            SaveGameDataToBinaryFile(gameData);
        }
    }
    #endregion

    #region || ----- Loading ----- ||

    public AllGameData LoadingTypeSwitch()
    {
        if (isSavingToJson)
        {
            AllGameData gameData = LoadGameDataFromJsonFile();
            return gameData;
        }
        else
        {
            AllGameData gameData = LoadGameDataFromBinaryFile();
            return gameData;
        }
    }

    public void LoadGame()
    {
        // Player Data
        SetPlayerData(LoadingTypeSwitch().playerData);

        //Environment Data




    }

    private void SetPlayerData(PlayerData playerData)
    {
        //Set Player Stats

        PlayerState.Instance.currentHealth = playerData.playerStats[0];
        PlayerState.Instance.currentCalories = playerData.playerStats[1];
        PlayerState.Instance.currentHydrationPercent = playerData.playerStats[2];


        // Set Player Position and Rotation
        Vector3 loadedPosition;
        loadedPosition.x = playerData.playerPositionAndRotation[0];
        loadedPosition.y = playerData.playerPositionAndRotation[1];
        loadedPosition.z = playerData.playerPositionAndRotation[2];

        PlayerState.Instance.playerBody.transform.position = loadedPosition;

        Vector3 loadedRotation;

        loadedRotation.x = playerData.playerPositionAndRotation[3];
        loadedRotation.y = playerData.playerPositionAndRotation[4];
        loadedRotation.z = playerData.playerPositionAndRotation[5];

        PlayerState.Instance.playerBody.transform.rotation = Quaternion.Euler(loadedRotation);



    }

    public void StartLoadedGame()
    {
        SceneManager.LoadScene("GameScene");

        StartCoroutine(DelayedLoading());
    }

    private IEnumerator DelayedLoading()
    {
        yield return new WaitForSeconds(1f);

        LoadGame();

    }

    #endregion


    #endregion


    #region || ----- To Binary Section ----- ||
    public void SaveGameDataToBinaryFile(AllGameData gameData)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(binaryPath, FileMode.Create);

        formatter.Serialize(stream, gameData);
        stream.Close();

        print("Game Data Saved to: " + binaryPath);


    }

    public AllGameData LoadGameDataFromBinaryFile()
    {

        if (File.Exists(binaryPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(binaryPath, FileMode.Open);
            AllGameData data = formatter.Deserialize(stream) as AllGameData;
            stream.Close();
            print("Game Data Loaded from: " + Application.persistentDataPath + "/save_game.bin");
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + binaryPath);
            return null;
        }
    }

    
    #endregion


    #region || ----- To Json Section ----- ||
    public void SaveGameDataToJsonFile(AllGameData gameData)
    {
        string json = JsonUtility.ToJson(gameData);

        string encrypted = EncryptDecrypt(json);

        using (StreamWriter writer = new StreamWriter(jsonPathProject))
        {
            writer.Write(encrypted);
            print("Game Data Saved to json file at: " + jsonPathProject);
        }


    }

    public AllGameData LoadGameDataFromJsonFile()
    {
        using (StreamReader reader = new StreamReader(jsonPathProject))
        {
            string json = reader.ReadToEnd();

            string decrypted = EncryptDecrypt(json);

            AllGameData data = JsonUtility.FromJson<AllGameData>(decrypted);
            print("Game Data Loaded from json file at: " + jsonPathProject);
            return data;
        }
    }

    
    #endregion


    #region || ----- Settings Section ----- ||
    #region || ----- Volume Settings ----- ||

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
    #endregion
    #endregion


    #region || ----- Encryption ----- ||

    public string EncryptDecrypt(string input)
    {
        string keyword = "1234567";
        string result = "";

        for (int i = 0; i < input.Length; i++)
        {
            result += (char)(input[i] ^ keyword[i % keyword.Length]);
        }

        return result;
    }

    #endregion


}
