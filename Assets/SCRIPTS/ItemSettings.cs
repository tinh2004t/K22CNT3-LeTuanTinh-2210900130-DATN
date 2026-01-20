using UnityEngine;

public class ItemSettings : MonoBehaviour
{
    [Header("Vị trí khi cầm trên tay (Local Transform)")]
    public Vector3 positionOffset;
    public Vector3 rotationOffset;
    public Vector3 scaleOffset = Vector3.one; // Mặc định là 1,1,1
}