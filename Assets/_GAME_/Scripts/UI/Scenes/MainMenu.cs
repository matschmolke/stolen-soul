using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        DontDestroyOnLoadCleaner.Clear();
        SceneManager.LoadScene("MainScene");
    }

    public void Continue()
    {
        SaveLoad.ContinueGame();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
