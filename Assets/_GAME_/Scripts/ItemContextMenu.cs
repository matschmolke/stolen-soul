using TMPro;
using UnityEngine;

public class ItemContextMenu : MonoBehaviour
{
    public static ItemContextMenu Instance;

    public bool IsActive => MenuObject.activeSelf;

    [SerializeField] private GameObject MenuObject;
    [SerializeField] private GameObject SplitButton;
    [SerializeField] private GameObject DeleteButton;

    private InventoryBase currentInventory;
    private int currentSlotId;

    private void Awake()
    {
        Instance = this;
        MenuObject.SetActive(false);
    }

    public void ShowContextMenu(InventoryBase inventory, int slotId, Vector3 pos, bool enableDelete = true)
    {
        bool isItemStackable = inventory.CanItemBeSplitted(slotId);

        if(!isItemStackable && !enableDelete)
        {
            // No options to show
            return;
        }

        DeleteButton.SetActive(enableDelete);

        SplitButton.SetActive(isItemStackable); 

        currentInventory = inventory;
        currentSlotId = slotId;

        MenuObject.transform.position = pos + new Vector3(100, -50, 0); // offset from slot
        MenuObject.SetActive(true);
    }

    public void HideContextMenu()
    {
        MenuObject.SetActive(false);
        currentInventory = null;
        currentSlotId = 0;
    }

    public void OnSplitButtonPressed()
    {
        currentInventory.SplitItemStack(currentSlotId);
        HideContextMenu();
    }

    public void OnDeleteButtonPressed()
    {
        currentInventory.RemoveItemAt(currentSlotId);
        HideContextMenu();
    }
}
