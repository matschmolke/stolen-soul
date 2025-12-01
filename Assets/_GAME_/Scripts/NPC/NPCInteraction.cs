using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    [Header("Dialogue")]
    public string[] dialogueLines;
    public DialogueNew dialogueSystem;

    [Header("Interaction")]
    public float interactDistance = 2f;
    private Transform player;

    private bool isInRange = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerTriggerCollider")?.transform;

        if (dialogueSystem == null)
        {
            dialogueSystem = FindFirstObjectByType<DialogueNew>();
            if (dialogueSystem == null)
                Debug.LogError("DialogueNew not found in scene!");
        }
    }

    private void Update()
    {
        if (player == null || dialogueSystem == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        isInRange = distance <= interactDistance;

        if (isInRange && Input.GetKeyDown(KeyCode.Space))
        {
            //dialogueSystem.StartDialogue(dialogueLines);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactDistance);
    }
}