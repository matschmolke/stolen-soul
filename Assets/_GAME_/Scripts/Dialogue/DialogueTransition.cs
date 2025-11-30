using UnityEngine;
using UnityEngine.UI;

public class DialogueTransition : MonoBehaviour
{
    [SerializeField] public Animator dialogeWin;
    [SerializeField] private Animator dialogeImg;

    private bool firstDialogue = true;

    private void Start()
    {
        Debug.Log("To disable the dialog, go to the SceneLoader script and comment out the Guide line.");
    }
    void Update()
    {
        if (Input.anyKey && firstDialogue)
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
    }

    public void hideDialogueWin()
    {
        dialogeWin.SetBool("show", false);
    }
}
