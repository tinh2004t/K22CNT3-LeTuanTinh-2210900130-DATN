using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeItemSlot : MonoBehaviour
{
    public UpgradeRecipeSO recipe;

    public Image itemImageUI;
    public TextMeshProUGUI itemNameUI;
<<<<<<< HEAD
    public TextMeshProUGUI costTextUI; 
=======
    public TextMeshProUGUI costTextUI; // Dùng để hiển thị danh sách nguyên liệu
>>>>>>> origin/main
    public Button upgradeButton;

    private void Start()
    {
        upgradeButton.onClick.AddListener(OnUpgradeClicked);
    }

    private void OnUpgradeClicked()
    {
<<<<<<< HEAD
=======
        // Khi bấm nút ở slot nào, nó sẽ gửi công thức của slot đó cho Manager xử lý
>>>>>>> origin/main
        UpgradeUIManager.Instance.AttemptUpgrade(recipe);
    }
}