using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTransition : MonoBehaviour
{
    [SerializeField] private Animator dialogeWin;
    [SerializeField] private Animator dialogeImg;

    public bool firstDialogue = false;
    public bool dialogueEnded = false;

    public DialogueEndEventChannel dialogueEndEvent;

    void Update()
    {
        if (firstDialogue)
        {
            dialogeWin.SetBool("show", true);
            firstDialogue = false;
        }
    }

    private void showImg()
    {
        dialogeImg.SetBool("show", true);
    }

    public void hideImg()
    {
        dialogeImg.SetBool("show", false);
        dialogueEndEvent.RaiseEvent();
    }

    public void hideDialogueWin()
    {
        dialogeWin.SetBool("show", false);
    }
}
