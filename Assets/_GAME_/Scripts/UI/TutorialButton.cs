using UnityEngine;

public class TutorialButton : MonoBehaviour
{
    public GameObject tutorialWindow;
    public static bool tutorialActive = true;

    private void Start()
    {
        if (GameState.RestoreFromSave)
           tutorialWindow.SetActive(false);
        else
           tutorialWindow.SetActive(true);
    }

    public void ShowTutorial()
    {
        if (!tutorialActive)
        {
            tutorialWindow.SetActive(true);
            tutorialActive = true;
            Time.timeScale = 0f;
        }
        else
        {
            tutorialWindow.SetActive(false);
            tutorialActive = false;
            Time.timeScale = 1f;
        }
    }
}
