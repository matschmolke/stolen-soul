using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeforeFortress : MonoBehaviour
{
    [SerializeField] private  DialogueEventChannel startDialogueMysterious;
    [SerializeField] private DialogueEndEventChannel endDialogueMysterious;
    [SerializeField] private DialogueData dialogueData;

    private bool playOnce = true;
    public static bool hidePauseButton = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && playOnce 
            && LocationManager.GetCurrentLocation() == "FinalBossDung")
        {
            playOnce = false;
            StartDialogue();
        }
    }

    private void Start()
    {
        SceneManager.LoadSceneAsync("GuideScene", LoadSceneMode.Additive);
        
    }

    private void StartDialogue()
    {
        FreezeGame.LockForDialogue();
        startDialogueMysterious.RaiseEvent(dialogueData);
    }

    private void DialogueEnded()
    {
        hidePauseButton = true;
        FreezeGame.UnlockAfterDialogue();
    }

    

    private void OnEnable()
    {
        if (endDialogueMysterious != null)
            endDialogueMysterious.OnEventRaised += DialogueEnded;
    }

    private void OnDisable()
    {
        if (endDialogueMysterious != null)
            endDialogueMysterious.OnEventRaised -= DialogueEnded;
    }



}
