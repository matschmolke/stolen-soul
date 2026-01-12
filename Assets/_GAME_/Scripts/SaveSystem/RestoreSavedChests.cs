using System.Collections.Generic;
using UnityEngine;

public static class RestoreSavedChests 
{
    public static void Restore(List<ChestData> chestData)
    {
        if (chestData == null || chestData.Count == 0)
            return;

        foreach (var data in chestData)
        {
            if (!ChestInventory.AllChests.TryGetValue(data.chestId, out var chest))
            {
                Debug.LogWarning($"Chest not found: {data.chestId}");
                continue;
            }

            foreach (var item in data.items)
            {
                ItemBase itemBase = ItemDatabase.Instance.GetItemByName(item.itemName); 
                if (itemBase == null)
                {
                    Debug.LogWarning($"Item not found: {item.itemName}");
                    continue;
                }

                chest.AddItemAt(item.slotId, itemBase, item.quantity);
            }

        }

        Debug.Log("Chests restored");
    }
}
