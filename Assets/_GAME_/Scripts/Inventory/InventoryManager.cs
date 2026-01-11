using System.ComponentModel;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public bool CanOpenInventory { get; set; } = true;

    public GameObject InventoryMenu;

    [Header("IMPORTANT!! \nequip and quick slots should be at the end of list")]
    public ItemSlot[] itemSlot;
    
    private bool menuActivated;

    private PlayerStats playerStats;

    private PlayerInventory playerInventory;
    private SpellCaster spellCaster;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        playerInventory = PlayerInventory.Instance;

        playerInventory.Initialize(itemSlot.Length);

        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i].Initialize(i);
        }

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerStats>();
        spellCaster = player.GetComponent<SpellCaster>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!CanOpenInventory) return; 
        
        if (Input.GetKeyDown(KeyCode.E) && menuActivated)
        {
            Time.timeScale = 1;
            InventoryMenu.SetActive(false);
            menuActivated = false;
            SoundManager.PlaySound(SoundType.INVENTORY_CLOSE);

            if (ItemTooltip.Instance != null)
                ItemTooltip.Instance.HideTooltip();

            playerStats.GetComponent<Movements>().canAttack = true;
        }
        else if (Input.GetKeyDown(KeyCode.E) && !menuActivated)
        {
            playerStats.GetComponent<Movements>().canAttack = false;

            Time.timeScale = 0;
            InventoryMenu.SetActive(true);
            menuActivated = true;
            SoundManager.PlaySound(SoundType.INVENTORY_OPEN);

            RefreshUI();
        }
    }

    public bool AddItem(ItemBase item, int quantity)
    {
       //Debug.Log("itemName = " + item.itemName + ", type = " + item.itemType + ", quantity = " + quantity);

       return playerInventory.AddItem(item, quantity);
    }

    public void UseItem(ItemBase item)
    {
        if (item is Consumable consumable)
        {
            switch (consumable.resourceType)
            {
                case ResourceType.Health:
                    SoundManager.PlaySound(SoundType.DRINK);
                    playerStats.Heal(consumable.restoreAmount);
                    break;
                case ResourceType.Mana:
                    SoundManager.PlaySound(SoundType.DRINK);
                    playerStats.RestoreMana(consumable.restoreAmount);
                    break;
                default:
                    Debug.Log("Invalid resource Type");
                    break;
            }
        }
        else if (item is Scroll scroll)
        {
            if (!scroll.isOneTimeUse)
            {
                Debug.Log($"Adding spell: {scroll.spellData.spellName}");
                spellCaster.AddSpell(scroll.spellData);
                SoundManager.PlaySound(SoundType.SCROLL_USE);
            }
            else
            {
                spellCaster.CastUtilitySpell(scroll.spellData);
                SoundManager.PlaySound(SoundType.PAPER);
            }
        }
    }

    public void RefreshUI()
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i].RefreshUI();
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
