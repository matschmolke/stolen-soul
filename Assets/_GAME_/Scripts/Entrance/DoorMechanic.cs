using UnityEngine;

public class DoorMechanic : MonoBehaviour
{
    private Animator doorAnim;
    [SerializeField] private Collider2D doorColl;
    [SerializeField] private GameEventChannel DoorChannel;
    [SerializeField] private int keyId = -1;

    private bool playerInRange = false;
    private bool isOpen = false;
    private bool canOpen = false;

    private void Awake()
    {
        doorAnim = GetComponent<Animator>();

        canOpen = keyId < 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerInRange)
        {
            if (!isOpen && canOpen)
            {
                OpenDoor();
            }
            else if (isOpen)
            {
                CloseDoor();
            }
        }
    }

    private void OpenDoor()
    {
        SoundManager.PlaySound(SoundType.DOOR_OPEN);
        isOpen = true;
        doorAnim.SetBool("open", true);
        DoorChannel.RaiseEvent(false,false);
    }

    private void CloseDoor()
    {
        SoundManager.PlaySound(SoundType.DOOR_CLOSE);
        isOpen = false;
        doorAnim.SetBool("open", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerTriggerCollider"))
        {
            playerInRange = true;

            if (!isOpen && !canOpen)
            {
                if (HasHey())
                {
                    canOpen = true;

                    DoorChannel.RaiseEvent(true, false);
                }
                else
                {
                    DoorChannel.RaiseEvent(true, true); 
                    canOpen = false;
                }
            }
            else if (!isOpen)
            {
                DoorChannel.RaiseEvent(true, false);
                canOpen = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerTriggerCollider"))
        {
            playerInRange = false;
            DoorChannel.RaiseEvent(false,false);
        }
    }

    private bool HasHey()
    {
        //checking if the key is in invetory
        return false; //for test
    }

    public void EnableDoor()
    {
        if (doorColl.isActiveAndEnabled)
        {
            doorColl.enabled =false;
        }
        else
        {
            doorColl.enabled = true;
        }
    }
}
