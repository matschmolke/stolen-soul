using System.Collections.Generic;
using UnityEngine;

public class ChestInventory : InventoryBase
{
    [SerializeField]
    public List<InventoryItem> items = new List<InventoryItem>();

    private void Awake()
    {
        Initalize();
    }

    private void Initalize()
    {
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
