using System.Collections.Generic;
using UnityEngine;

public static class RestoreSavedChests 
{
    public static void Restore(List<ChestData> chestsData)
    {
        if (chestsData == null || chestsData.Count == 0)
        {
            Debug.Log("No chests to restore.");
            return;
        }

        foreach (var chestData in chestsData)
        {
            if (!ChestInventory.AllChests.TryGetValue(chestData.chestId, out var chest))
            {
                Debug.LogWarning($"Chest not found in scene: {chestData.chestId}");
                continue;
            }

            // Clear chest
            for (int i = 0; i < chest.SlotCount(); i++)
            {
                chest.RemoveItemAt(i);
            }

            // Restore items
            foreach (var itemData in chestData.items)
            {
                ItemBase item = ItemDatabase.Instance.GetItemByName(itemData.itemName);
                if (item == null)
                {
                    Debug.LogWarning($"Item not found in database: {itemData.itemName}");
                    continue;
                }

                chest.AddItemAt(
                    itemData.slotId,
                    item,
                    itemData.quantity
                );
            }
        }

        Debug.Log("All chests restored from save.");
    }

}