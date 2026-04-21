using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct MaterialRequirement
{
    public string itemName;
    public int amount;
}

[CreateAssetMenu(fileName = "New Upgrade Recipe", menuName = "Survival Game/Upgrade Recipe")]
public class UpgradeRecipeSO : ScriptableObject
{
    [Header("Yêu cầu")]
    public GameObject baseItemPrefab; // Kéo thả prefab UI đồ gốc vào đây
    public int coinCost;
    public List<MaterialRequirement> requiredMaterials;

    [Header("Kết quả")]
    public GameObject upgradedItemPrefab; // Kéo thả prefab UI đồ nâng cấp vào đây (Để lấy Tên và Icon)

    [Header("Hiệu ứng")]
    public GameObject upgradeEffect;
}