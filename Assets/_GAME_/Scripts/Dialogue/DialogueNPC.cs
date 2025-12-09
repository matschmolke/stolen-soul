using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueNPC : MonoBehaviour
{
    [SerializeField] private GameEventChannel DialogueChannel;
    [SerializeField] private Text text;
    [SerializeField] private GameObject dialogueUI;

    [SerializeField] private bool isTrader = false;

    [Header("If dialogue line is to be custom")]
    [SerializeField] private bool customText = false;
    [SerializeField] private string dialogueText;

    private float textSpeed = 0.05f;
    private bool inRange = false;

    private bool textFinished = false;

    private void Start()
    {
        text.text = string.Empty;
        dialogueUI.SetActive(false);
    }   

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && inRange)
        {
            DialogueChannel.RaiseEvent(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            DialogueChannel.RaiseEvent(true);

            if (!textFinished)
            {
                ShowMessage();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            DialogueChannel.RaiseEvent(false);
        }
    }

    private IEnumerator TypeLine(string line)
    {
        foreach (char c in line)
        {
            text.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void ShowMessage()
    {
        textFinished = true;

        if(dialogueUI.activeSelf == false)
        {

            dialogueUI.SetActive(true);
        }

        string message = isTrader ? "Hello traveler! Want to trade?" : "Hello traveler!";

        if (customText)
        {
            message = dialogueText;
        }

        StartCoroutine(TypeLine(message));
    }
}
