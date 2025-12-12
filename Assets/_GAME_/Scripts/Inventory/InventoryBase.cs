using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public ItemBase Item;
    public int Quantity;
    public bool IsFull => Quantity >= Item.maxStackSize;

    public InventoryItem()
    {
        Item = null;
        Quantity = 0;
    }

    public InventoryItem(ItemBase item, int quantitiy)
    {
        Item = item;
        Quantity = quantitiy;
    }

    public InventoryItem(InventoryItem other)
    {
        Item = other.Item;
        Quantity = other.Quantity;
    }
}

public abstract class InventoryBase : MonoBehaviour
{
    public Dictionary<int, InventoryItem> slots = new Dictionary<int, InventoryItem>();

    public void AddItemAt(int index, ItemBase Item, int quantity)
    {
        slots[index] = new InventoryItem(Item, quantity);
    }

    public bool AddItem(ItemBase item, int quantity)
    {
        int maxStack = item.maxStackSize;

        foreach (var key in slots.Keys)
        {
            var slot = slots[key];

            if (slot.Item == item && !slot.IsFull)
            {
                int freeSpace = maxStack - slot.Quantity;
                int addAmount = Mathf.Min(freeSpace, quantity);

                slot.Quantity += addAmount;
                quantity -= addAmount;

                if (quantity <= 0)
                    return true;
            }
        }

        foreach (var key in slots.Keys)
        {
            if (slots[key].Item == null)
            {
                int addAmount = Mathf.Min(maxStack, quantity);

                slots[key] = new InventoryItem(item, addAmount);
                quantity -= addAmount;

                if (quantity <= 0)
                    return true;
            }
        }
        return false;
    }

    public void ChangeQuantity(int index, int quantity)
    {
        slots[index].Quantity = quantity;
    }

    public void RemoveItemAt(int index)
    {
        slots[index] = new InventoryItem();
    }

    public int RemoveItem(ItemBase item, int quantity)
    {
        int removed = 0;

        foreach (var key in slots.Keys.ToList())
        {
            if (removed >= quantity)
                break;

            var slot = slots[key];

            if (slot.Item != item || slot.Quantity <= 0)
                continue;

            int toRemove = Mathf.Min(slot.Quantity, quantity - removed);

            
            slot.Quantity -= toRemove;
            removed += toRemove;

            
            if (slot.Quantity == 0)
                slots[key] = new InventoryItem();
            else
                slots[key] = slot; 
        }

        return removed;
    }

    public void MoveItem(int targetSlotId, int originSlotId)
    {
        if (!slots.ContainsKey(targetSlotId) || !slots.ContainsKey(originSlotId)) return;

        var target = slots[targetSlotId];
        var origin = slots[originSlotId];

        if (origin.Item == null) return; // nic do przeniesienia

        // Slot pusty – przenieœ ca³y przedmiot
        if (target.Item == null)
        {
            AddItemAt(targetSlotId, origin.Item, origin.Quantity);

            RemoveItemAt(originSlotId);
        }
        // Slot zawiera ten sam przedmiot – stackowanie
        else if (target.Item == origin.Item)
        {
            int maxStack = target.Item.maxStackSize;
            int totalQuantity = target.Quantity + origin.Quantity;

            if (totalQuantity <= maxStack)
            {
                ChangeQuantity(targetSlotId, totalQuantity);
                RemoveItemAt(originSlotId);
            }
            else
            {
                ChangeQuantity(targetSlotId, maxStack);
                ChangeQuantity(originSlotId, totalQuantity - maxStack);
            }
        }
        // Slot zawiera inny przedmiot – swap
        else
        {
            SwapItems(targetSlotId, originSlotId);
        }
    }

    public void SwapItems(int indexA, int indexB)
    {
        (slots[indexA], slots[indexB]) = (slots[indexB], slots[indexA]);
    }

    public InventoryItem GetItem(int index)
    {
        return slots[index];
    }

    public int GetQuantityOf(ItemBase item)
    {
        int totalQuantity = 0;

        foreach(var value in slots.Values)
        {
            if(value.Item == item)
            {
                totalQuantity += value.Quantity;
            }
        }

        return totalQuantity;
    }

    public abstract int SlotCount();
}
