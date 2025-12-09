using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public DialogueEventChannel dialogueEvent;
    public DialogueNew dialogueNew;
    public DialogueTransition transition;

    private void OnEnable()
    {
        dialogueEvent.OnEventRaised += HandleDialogueEvent;
    }

    private void OnDisable()
    {
        dialogueEvent.OnEventRaised -= HandleDialogueEvent;
    }

    private void HandleDialogueEvent(DialogueData data)
    {
        transition.firstDialogue = true;

        dialogueNew.LoadDialogue(data);
    }
}
