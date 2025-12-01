using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTransition : MonoBehaviour
{
    [SerializeField] public Animator dialogeWin;
    [SerializeField] private Animator dialogeImg;

    public bool firstDialogue = false;
    public bool dialogueEnded = false;

    private CharacterAppear characterAppear;

    void Start()
    {
        characterAppear = FindFirstObjectByType<CharacterAppear>();
    }

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
        StartCoroutine(characterAppear.Disappear());
    }

    public void hideDialogueWin()
    {
        dialogeWin.SetBool("show", false);
    }
}
