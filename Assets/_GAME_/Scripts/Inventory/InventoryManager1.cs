using UnityEngine;

public class InventoryManager1 : MonoBehaviour
{
    public static InventoryManager1 Instance { get; private set; }
    public GameObject InventoryMenu;
    public ItemSlot1[] itemSlot;
    
    private bool menuActivated;

    private PlayerStats1 playerStats;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerStats1>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && menuActivated)
        {
            Time.timeScale = 1;
            InventoryMenu.SetActive(false);
            menuActivated = false;
            if (ItemTooltip.Instance != null)
                ItemTooltip.Instance.HideTooltip();
        }
        else if (Input.GetKeyDown(KeyCode.E) && !menuActivated)
        {
            Time.timeScale = 0;
            InventoryMenu.SetActive(true);
            menuActivated = true;
        }
    }

    public bool AddItem(ItemBase item, int quantity)
    {
        Debug.Log("itemName = " + item.itemName + ", type = " + item.itemType + ", quantity = " + quantity);

        for(int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].Item == null) continue;                     
            if (itemSlot[i].Item.Id != item.Id) continue;               
            if (itemSlot[i].IsFull) continue;
            if (!itemSlot[i].CanAcceptItem(item)) continue;

            int totalQuantity = itemSlot[i].quantity + quantity;

            if (totalQuantity <= item.maxStackSize)
            {
                itemSlot[i].AddItem(item, quantity);
                return true;
            }
            else
            {
                int quantityToAdd = item.maxStackSize - itemSlot[i].quantity;
                itemSlot[i].AddItem(item, quantityToAdd);
                quantity -= quantityToAdd;
            }
        }

        for (int i = 0; i < itemSlot.Length; i++)
        {
            if(itemSlot[i].Item != null) continue;
            if (!itemSlot[i].CanAcceptItem(item)) continue;

            itemSlot[i].AddItem(item, quantity);
            return true;
        }

        Debug.Log("Inventory Full!");
        return false;
    }

    public void UseItem(ItemBase item)
    {
        if(item is Consumable consumable)
        {
            switch (consumable.resourceType)
            {
                case ResourceType.Health:
                    playerStats.Heal(consumable.restoreAmount);
                    break;
                case ResourceType.Mana:
                    playerStats.RestoreMana(consumable.restoreAmount);
                    break;
                default:
                    Debug.Log("Invalid resource Type");
                    break;
            }
        }
    }

    public void DeselectAllSlots()
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i].selectedShader.SetActive(false);
            itemSlot[i].thisItemSelected = false;
        }
    }
}
