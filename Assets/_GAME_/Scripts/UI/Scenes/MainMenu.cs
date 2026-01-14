using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        GameState.Clear();
        DontDestroyOnLoadCleaner.Clear();
        SceneManager.LoadScene("MainScene");
    }

    public void Continue()
    {
        if (GameState.TryLoadGame())
        {
            SaveLoad.StartContinue();
        }
        else
        {
            Debug.Log("Save not found - starting new game");
            NewGame();
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
