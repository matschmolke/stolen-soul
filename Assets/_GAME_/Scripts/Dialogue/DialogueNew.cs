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

    //private bool triggerDialogue = true;

    /*private bool timeUp = false;
    private float time = 3f;*/

    void Start()
    {
        text.text = string.Empty;
        StartDialogue();
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

        /*if (timeUp) {

            time -= Time.deltaTime;

            if (time < 0)
            {
                timeUp = false;
                StartDialogue();
            }
        }*/
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
        
        //triggerDialogue = false;
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
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
            //koniec dialogu
            gameObject.SetActive(false);
            text.text = string.Empty;
            
        }
    }

    
}
