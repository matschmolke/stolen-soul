using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : InventoryBase
{
    public static PlayerInventory Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void Initialize(int numberOfSlots)
    {
        for(int i = 0; i < numberOfSlots; i++)
        {
            slots[i] = new InventoryItem();
        }
    }

    public override int SlotCount()
    {
        //-4 because we skip equipSlots and quickSlots (it could be done better)
        return slots.Count -4;
    }
}
