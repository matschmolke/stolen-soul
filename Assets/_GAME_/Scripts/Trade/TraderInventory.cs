using System.Collections.Generic;
using UnityEngine;

public class TraderInventory : InventoryBase
{
    public static TraderInventory Instance;

    [SerializeField]
    public List<InventoryItem> items = new List<InventoryItem>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Initalize();
        } 
        else
            Destroy(gameObject);
    }

    private void Initalize()
    {
        for(int i = 0; i < items.Count; i++)
        {
            slots.Add(i, items[i]);
        }
    }

    public override int SlotCount()
    {
        return slots.Count;
    }
}
