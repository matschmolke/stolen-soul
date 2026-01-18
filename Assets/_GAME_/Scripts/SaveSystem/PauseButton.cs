using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseButton : MonoBehaviour
{
    public static bool pause = false;

    public GameObject pauseButton;

    private void Start()
    {
        if (pauseButton == null)
            pauseButton = GameObject.FindGameObjectWithTag("PauseButton");
    }

    private void Update()
    {
        if (FreezeGame.DialogueActive)
            pauseButton.SetActive(false);
        else
            pauseButton.SetActive(true);
    }

    public void PauseGame(bool pause)
    {
        if (pause)
        {
            SceneManager.LoadSceneAsync("PauseScene", LoadSceneMode.Additive);
            Time.timeScale = 0f;
        }
        else
        {
            SceneManager.UnloadSceneAsync("PauseScene");
            Time.timeScale = 1f;
        }

        pause = !pause;
    }

    public void SaveGame()
    {
        SaveLoad.SaveGame();
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        DontDestroyOnLoadCleaner.Clear();
        SceneManager.LoadScene("StartScene");
    }
}
