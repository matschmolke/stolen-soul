using UnityEngine;
using UnityEngine.SceneManagement;

public class YouDied : MonoBehaviour
{
    public void RestartLevel()
    {
        DontDestroyOnLoadCleaner.Clear();
        SceneManager.LoadScene("MainScene");
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("StartScene");
    }


}
