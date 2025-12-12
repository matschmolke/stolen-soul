using System.Collections.Generic;
using UnityEngine;

public class TraderInventory : InventoryBase
{
    //public static TraderInventory Instance;

    //[Header("List must contain 24 entries. Entries can be empty")]
    private List<InventoryItem> items = new List<InventoryItem>();

    //private void Awake()
    //{
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //        Initalize();
    //    } 
    //    else
    //        Destroy(gameObject);
    //}

    public void Initalize(TraderInventoryData inventoryData)
    {
        foreach (var item in inventoryData.startingItems)
        {
            items.Add(new InventoryItem(item));
        }

        for (int i = 0; i < items.Count; i++)
        {
            slots.Add(i, items[i]);
        }
    }

    public override int SlotCount()
    {
        return slots.Count;
    }
}
