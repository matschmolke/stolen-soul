using UnityEngine;
using UnityEngine.UI;

public class EntranceMechanic : MonoBehaviour
{
    [SerializeField] private Text gameInfo;

    private Animator entranceAnim;
    private bool playerInRange = false;

    private BoxCollider2D entranceColl;

    private void Awake()
    {
        entranceAnim = GetComponent<Animator>();
        entranceColl = GetComponent<BoxCollider2D>();
        if (gameInfo != null)
            gameInfo.enabled = false;

    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.O) && playerInRange)
        {
            EntranceOpen();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameInfo.enabled = true;
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameInfo.enabled = false;
            playerInRange = false;
        }
    }

    private void EntranceOpen()
    {
        if (entranceAnim != null && entranceAnim.runtimeAnimatorController != null)
        {
            entranceAnim.SetTrigger("open");
            if (entranceColl != null)
                entranceColl.enabled = false;
        }
    }
}
