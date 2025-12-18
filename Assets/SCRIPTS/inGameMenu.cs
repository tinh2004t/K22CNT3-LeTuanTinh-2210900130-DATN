using UnityEngine;
using UnityEngine.SceneManagement;

public class inGameMenu : MonoBehaviour
{
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }


}
