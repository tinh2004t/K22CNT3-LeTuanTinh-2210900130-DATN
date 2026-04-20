using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeItemSlot : MonoBehaviour
{
    public UpgradeRecipeSO recipe;

    public Image itemImageUI;
    public TextMeshProUGUI itemNameUI;
    public TextMeshProUGUI costTextUI; // Dùng để hiển thị danh sách nguyên liệu
    public Button upgradeButton;

    private void Start()
    {
        upgradeButton.onClick.AddListener(OnUpgradeClicked);
    }

    private void OnUpgradeClicked()
    {
        // Khi bấm nút ở slot nào, nó sẽ gửi công thức của slot đó cho Manager xử lý
        UpgradeUIManager.Instance.AttemptUpgrade(recipe);
    }
}