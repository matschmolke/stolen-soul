using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueNew : MonoBehaviour
{
    private Text text;
    private Image dialogueImg;

    [Header("Scriptable Object Dialogue")]
    public DialogueData dialogue;

    public float textSpeed = 0.08f;

    private int index;

    [SerializeField] private DialogueTransition transition;

    private void Awake()
    {
        text = GameObject.FindGameObjectWithTag("DialogueWindow").GetComponent<Text>();
        dialogueImg = GameObject.FindGameObjectWithTag("DialogueImage").GetComponent<Image>();
    }
    void Start()
    {
        text.text = string.Empty;

        if (dialogue != null)
        {
            //LoadDialogue(dialogue);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            if (text.text == dialogue.lines[index] && dialogue != null)
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                text.text = dialogue.lines[index];
            }
        }

    }

    public void LoadDialogue(DialogueData data)
    {
        dialogue = data;

        text.text = string.Empty;
        if (dialogueImg != null && dialogue.image != null)
        {
            dialogueImg.sprite = dialogue.image;
        }
    }

    public void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in dialogue.lines[index])
        {
            text.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < dialogue.lines.Count - 1)
        {
            index++;
            text.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            //The end of the dialogue
            transition.hideDialogueWin();
            text.text = string.Empty;

        }
    }

}
