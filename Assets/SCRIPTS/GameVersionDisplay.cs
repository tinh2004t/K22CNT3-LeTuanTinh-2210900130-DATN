using UnityEngine;
using TMPro;

public class GameVersionDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI versionText;

    private void Start()
    {
        if (versionText != null)
        {
            versionText.text = "Version " + Application.version;
        }
        else
        {
            Debug.LogWarning("Chưa gán VersionText vào script!");
        }
    }
}