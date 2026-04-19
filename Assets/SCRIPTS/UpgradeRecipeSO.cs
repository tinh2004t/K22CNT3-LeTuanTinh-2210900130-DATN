using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct MaterialRequirement
{
    public ItemSO item; // Cấu trúc ItemSO hiện tại của game bạn
    public int amount;
}

[CreateAssetMenu(fileName = "New Upgrade Recipe", menuName = "Survival Game/Upgrade Recipe")]
public class UpgradeRecipeSO : ScriptableObject
{
    public string recipeName;
    public ItemSO baseItem;             // Vật phẩm gốc (VD: Kiếm sắt)
    public ItemSO upgradedItem;         // Vật phẩm sau khi nâng cấp (VD: Kiếm thép)

    public int coinCost;                // Số vàng yêu cầu
    public List<MaterialRequirement> requiredMaterials; // Danh sách nguyên liệu

    public GameObject upgradeEffect;    // Hiệu ứng hạt (Particle) khi nâng cấp thành công
}