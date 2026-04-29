using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeItemSlot : MonoBehaviour
{
    public UpgradeRecipeSO recipe;

    public Image itemImageUI;
    public TextMeshProUGUI itemNameUI;
    public TextMeshProUGUI costTextUI; 
    public Button upgradeButton;

    private void Start()
    {
        upgradeButton.onClick.AddListener(OnUpgradeClicked);
    }

    private void OnUpgradeClicked()
    {
        UpgradeUIManager.Instance.AttemptUpgrade(recipe);
    }
}