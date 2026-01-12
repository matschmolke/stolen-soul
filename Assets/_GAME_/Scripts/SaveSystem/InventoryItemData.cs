using UnityEngine;

[System.Serializable]
public class InventoryItemData
{
    public string itemName;
    public int quantity;
    public int slotId;

    public InventoryItemData(string itemName, int quantity, int slotId)
    {
        this.itemName = itemName;
        this.quantity = quantity;
        this.slotId = slotId;
    }
}
