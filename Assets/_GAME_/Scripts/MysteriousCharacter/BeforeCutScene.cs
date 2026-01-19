using System.Collections;
using UnityEngine;

public class BeforeCutScene : MonoBehaviour
{
    private bool hasShownTutorial = false;
    private bool firstTime = true;

    public static bool showTutoialFirstTime = true;

    void Update()
    {
        if (GameState.RestoreFromSave)
            return;

        if (!hasShownTutorial && !TutorialButton.tutorialActive && firstTime && TutorialButton.tutorialEnabled)
        {
            firstTime = false;
            hasShownTutorial = true;
            CharacterAppear.StartCutscene = true;
        }
    }


}
