using System.Collections.Generic;
using UnityEngine;

public class ChestInventory : InventoryBase
{
    public static Dictionary<string, ChestInventory> AllChests = new();

    [Header("unique Id")]
    public string chestId;

    [Header("List must contain 8 entries. Entries can be empty")]
    public List<InventoryItem> items = new List<InventoryItem>();

    private void Awake()
    {
        if (string.IsNullOrEmpty(chestId))
        {
            Debug.LogError("Chest has no chestId", this);
            return;
        }

        AllChests[chestId] = this;

        Initalize();
    }

    private void Initalize()
    {
        slots.Clear();

        for (int i = 0; i < items.Count; i++)
        {
            slots.Add(i, items[i]);
        }

        //restore chests from save
        if (GameState.RestoreFromSave && GameState.LoadedData != null)
        {
            RestoreSavedChests.Restore(GameState.LoadedData.chests);
        }
        
    }

    public override int SlotCount()
    {
        return slots.Count;
    }
}
