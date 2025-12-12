using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NPC/TraderInventoryData")]
public class TraderInventoryData : ScriptableObject
{
    public static readonly int numberOfSlots = 24;

    public List<InventoryItem> startingItems = new List<InventoryItem>(numberOfSlots);

    private void OnValidate()
    {
        if(startingItems.Count != numberOfSlots)
        {
            Debug.LogWarning($"TraderInventoryData must have exactly {numberOfSlots} starting items. Current count: {startingItems.Count}");
            while (startingItems.Count < numberOfSlots)
            {
                startingItems.Add(new InventoryItem());
            }
            while (startingItems.Count > numberOfSlots)
            {
                startingItems.RemoveAt(startingItems.Count - 1);
            }
        }
    }
}
