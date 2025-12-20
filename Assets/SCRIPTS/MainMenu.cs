using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button LoadGameBTN;

    private void Start()
    {
        LoadGameBTN.onClick.AddListener(() =>
        {
            SaveManager.Instance.StartLoadedGame();
        });
    }

    public void NewGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }
}
