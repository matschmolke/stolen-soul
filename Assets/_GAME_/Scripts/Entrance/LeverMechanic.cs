using UnityEditor.Rendering;
using UnityEngine;

public class LeverMechanic : MonoBehaviour
{

    [SerializeField] private EntranceMechanic targetDoor;
    [SerializeField] private GameEventChannel EntranceChannel;

    private Animator leverAnim;

    private bool playerInRange = false;
    private bool isOpen = false;

    private void Awake()
    {
        leverAnim = GetComponent<Animator>();

        if (targetDoor == null)
        {
            Debug.LogError("No entrance is assigned to the lever: " + gameObject.name);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerInRange)
        {
            if (!isOpen) 
            {
                TurnOnLever();
            }
            else
            {
                TurnOffLever();
            }
            
        }
    }
    private void TurnOnLever()
    {
        isOpen = true;
        leverAnim.SetBool("turnOn", true);
        EntranceChannel.RaiseEvent(false);
    }

    private void TurnOffLever()
    {
        isOpen = false;
        leverAnim.SetBool("turnOn", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerTriggerCollider"))
        {
            playerInRange = true;

            if (!isOpen)
            {
                EntranceChannel.RaiseEvent(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerTriggerCollider"))
        {
            playerInRange = false;
            EntranceChannel.RaiseEvent(false);
        }
    }


    private void OpenEntrance()
    {
        targetDoor.EntranceOpen();
    }

    private void CloseEntrance()
    {
        targetDoor.EntranceClose();
    }
}
