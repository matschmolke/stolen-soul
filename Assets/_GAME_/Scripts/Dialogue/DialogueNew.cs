using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueNew : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Text text;
    [SerializeField] private DialogueTransition transition;

    [Header("Dialogue Lines")]
    [SerializeField] private string[] lines;
    [SerializeField] private float textSpeed = 0.08f;

    private int index = 0;
    private bool isTyping = false;

    private void Start()
    {
        if (text == null)
        {
            Debug.LogError($"{name}: Text component is not assigned!");
        }

        if (transition == null)
        {
            Debug.LogWarning($"{name}: DialogueTransition not assigned.");
        }

        text.text = string.Empty;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (lines == null || lines.Length == 0) return;

            if (!isTyping)
            {
                NextLine();
            }
            else
            {
                // закінчити набір рядка миттєво
                StopAllCoroutines();
                text.text = lines[index];
                isTyping = false;
            }
        }
    }

    public void StartDialogue(string[] dialogueLines)
    {
        if (dialogueLines == null || dialogueLines.Length == 0) return;

        lines = dialogueLines;
        index = 0;

        if (transition != null)
            transition.dialogeWin.SetBool("show", true);

        StartCoroutine(TypeLine());
    }

    private IEnumerator TypeLine()
    {
        if (lines == null || index >= lines.Length) yield break;

        isTyping = true;
        text.text = "";

        foreach (char c in lines[index])
        {
            text.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
    }

    private void NextLine()
    {
        index++;

        if (index < lines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        text.text = string.Empty;
        if (transition != null)
            transition.hideDialogueWin();
    }
}
