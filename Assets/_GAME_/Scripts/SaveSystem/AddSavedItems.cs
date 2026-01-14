using UnityEngine;

public static class AddSavedItems
{
    public static void RestoreInventory(PlayerData data)
    {
        if (data?.inventoryItems == null || data.inventoryItems.Count == 0)
        {
            Debug.Log("No inventory items to restore.");
            return;
        }

        var playerInventory = PlayerInventory.Instance;
        var inventoryManager = InventoryManager.Instance;

        if (playerInventory == null || inventoryManager == null)
        {
            Debug.LogWarning("Inventory not ready yet.");
            return;
        }

        foreach (var itemData in data.inventoryItems)
        {
            ItemBase item = ItemDatabase.Instance.GetItemByName(itemData.itemName);
            if (item == null)
            {
                Debug.LogWarning($"Item not found: {itemData.itemName}");
                continue;
            }

            playerInventory.AddItemAt(
                itemData.slotId,
                item,
                itemData.quantity
            );

            //the same result
            //inventoryManager.AddItem(item, itemData.quantity);
        }

        inventoryManager.RefreshUI();
        Debug.Log("Inventory restored from save.");
    }
}
