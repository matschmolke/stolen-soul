using UnityEngine;

public class ChestMechanic : MonoBehaviour
{
    [SerializeField] private GameEventChannel ChestChannel;

    private Animator chestAnim;

    private bool playerInRange = false;
    private bool isOpen = false;

    private void Awake()
    {
        chestAnim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerInRange)
        {
            if (!isOpen) 
            {
                OpenChest();
            }
            else
            {
                HideChestUI();
            }
        }
    }
    private void OpenChest()
    {
        isOpen = true;
        chestAnim.SetBool("open", true);
        ChestChannel.RaiseEvent(false);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerTriggerCollider"))
        {
            playerInRange = true; 
            if (!isOpen)
            {
                ChestChannel.RaiseEvent(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerTriggerCollider"))
        {
            playerInRange = false;
            ChestChannel.RaiseEvent(false);
        }
    }


    private void ShowChestUI()
    {
        Debug.Log("Chest UI is showing");
        Time.timeScale = 0;
        //code to activate show chest UI
    }

    private void HideChestUI()
    {
        Debug.Log("Chest UI hidden");
        Time.timeScale = 1;
        chestAnim.SetBool("open", false);
        isOpen = false;
        //code to activate show chest UI
    }
}
