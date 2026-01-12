using System.IO;
using UnityEditor.Overlays;
using UnityEngine;

public static class AddSavedItems
{
    public static void RestoreInventory(PlayerData data)
    {
        if (data == null || data.inventoryItems == null || data.inventoryItems.Count == 0)
        {
            Debug.LogWarning("No inventory items to restore.");
            return;
        }

        var playerInventory = PlayerInventory.Instance;
        var inventoryManager = InventoryManager.Instance;

        if (playerInventory == null || inventoryManager == null)
        {
            Debug.LogError("Inventory not initialized.");
            return;
        }

        foreach (var itemData in data.inventoryItems)
        {
            ItemBase item = ItemDatabase.Instance.GetItemByName(itemData.itemName);
            if (item == null)
            {
                Debug.LogWarning($"Item not found in database: {itemData.itemName}");
                continue;
            }

            playerInventory.AddItemAt(itemData.slotId, item, itemData.quantity);
        }

        inventoryManager.RefreshUI();

        Debug.Log("Inventory restored from PlayerData.");
    }
}
