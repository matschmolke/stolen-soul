using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueData dialogue;
    public DialogueEventChannel dialogueEvent;

    private bool triggered = false;
    private bool inRange = false;

    [Header("Trigger Dialogue on collision")]
    [SerializeField] private bool startOnCollision = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && inRange)
        {
            triggered = true;
            dialogueEvent.RaiseEvent(dialogue);
        }
    }

    //if you want to trigger dialogue on player collision
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag("Player") && startOnCollision)
        {
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && startOnCollision)
        {
            inRange = false;
        }
    }


    //if you want to trigger dialogue in other scripts
    public void StartDialogue()
    {
        dialogueEvent.RaiseEvent(dialogue);
    }

}
