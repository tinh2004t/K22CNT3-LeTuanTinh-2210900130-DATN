using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;


[System.Serializable]
public class PlayerData
{
    public float[] playerStats; // [0] health, [1] calories, [2] hydration

    public float[] playerPositionAndRotation; // position x,y,z [3] rotation x,y,z
    public string[] inventoryContent;
    public string[] quickSlotcontent;

    public PlayerData(float[] _playerStats, float[] _playerPosAndRot, string[] _inventoryContent, string[] _quickSlotcontent)
    {
        playerStats = _playerStats;
        playerPositionAndRotation = _playerPosAndRot;
        inventoryContent = _inventoryContent;
        quickSlotcontent = _quickSlotcontent;
    }




}
