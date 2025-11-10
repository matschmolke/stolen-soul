using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueNew : MonoBehaviour
{
    public Text text;
    public string[] lines;
    public float textSpeed = 0.08f;

    private int index;

    [SerializeField] private DialogueTransition transition;
    void Start()
    {
        text.text = string.Empty;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            if (text.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                text.text = lines[index];
            }
        }

    }

    public void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index])
        {
            text.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length-1)
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
