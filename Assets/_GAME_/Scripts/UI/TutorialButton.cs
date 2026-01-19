using System.Collections;
using UnityEngine;

public class TutorialButton : MonoBehaviour
{
    public GameObject tutorialWindow;
    public static bool tutorialActive = false;

    public static bool tutorialEnabled = false;

    private void Start()
    {
        StartCoroutine(waitToShow(3f));
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

    private IEnumerator waitToShow(float wait)
    {
        yield return new WaitForSeconds(wait);

        if (BeforeCutScene.showTutoialFirstTime && !GameState.RestoreFromSave)
        {
            BeforeCutScene.showTutoialFirstTime = false;
            ShowTutorial();

            tutorialEnabled = true;
        }
        
    }
}
