using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameScene : MonoBehaviour
{
    public DialogueData dialogue;
    public DialogueEventChannel dialogueEvent;
    public DialogueEndEventChannel finalEndEvent;

    public EnemyAI endBoss;

    private bool dialogueStarted = false;

    private void Start()
    {
        SceneManager.LoadSceneAsync("GuideScene", LoadSceneMode.Additive);
    }

    private IEnumerator StartDialogue()
    {
        yield return new WaitForSeconds(2f);
        dialogueEvent.RaiseEvent(dialogue);
    }


    public void PickUpMemoryOrb()
    {
        FreezeGame.LockForDialogue();

        StartCoroutine(StartDialogue());
    }
    private void DialogueEnded()
    {
        ScreenFader fader = FindObjectOfType<ScreenFader>();
        fader.FadeToBlack();
    }

    private void OnEnable()
    {
        if (finalEndEvent != null)
            finalEndEvent.OnEventRaised += DialogueEnded;
    }

    private void OnDisable()
    {
        if (finalEndEvent != null)
            finalEndEvent.OnEventRaised -= DialogueEnded;
    }

}
