using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChestData
{
    public string chestId;
    public List<InventoryItemData> items = new();
}
