using System.ComponentModel;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
    public static ChestManager Instance { get; private set; }
    public GameObject ChestMenu;

    public ChestSlot[] PlayerInvSlots;
    public ChestSlot[] ChestInvSlots;

    [HideInInspector]
    public ChestInventory chestInventory;

    private bool menuActivated;

    private PlayerInventory playerInventory;

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

        foreach(var slot in PlayerInvSlots)
        {
            slot.Inventory = playerInventory;
            slot.Owner = SlotOwner.Player;
        }

        foreach(var slot in ChestInvSlots)
        {
            slot.Owner = SlotOwner.Chest;
        }
    }

    public void OpenChestUI(ChestInventory chestInv)
    {
        playerInventory.GetComponent<Movements>().canAttack = false;

        chestInventory = chestInv;
        for (int i = 0; i < ChestInvSlots.Length; i++)
        {
            ChestInvSlots[i].Inventory = chestInventory;
        }
        RefreshUI();
        Time.timeScale = 0;
        SoundManager.PlaySound(SoundType.CHEST_OPEN);
        ChestMenu.SetActive(true);
        menuActivated = true;
    }

    public void CloseChestUI()
    {
        Time.timeScale = 1;
        ChestMenu.SetActive(false);
        SoundManager.PlaySound(SoundType.CHEST_CLOSE);
        menuActivated = false;
        if (ItemTooltip.Instance != null)
            ItemTooltip.Instance.HideTooltip();

        playerInventory.GetComponent<Movements>().canAttack = true;
    }

    public void RefreshUI()
    {
        for (int i = 0; i < PlayerInvSlots.Length; i++)
        {
            PlayerInvSlots[i].RefreshUI();
        }

        for (int i = 0; i < ChestInvSlots.Length; i++)
        {
            ChestInvSlots[i].RefreshUI();
        }
    }

    public void DeselectAllSlots()
    {
        for (int i = 0; i < PlayerInvSlots.Length; i++)
        {
            PlayerInvSlots[i].thisItemSelected = false;
        }

        for (int i = 0; i < ChestInvSlots.Length; i++)
        {
            ChestInvSlots[i].thisItemSelected = false;
        }
    }
}
