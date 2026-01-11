using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        DontDestroyOnLoadCleaner.Clear();
        SceneManager.LoadScene("MainScene");
    }

    public void LoadGame()
    {
        Debug.Log("Loading game");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
