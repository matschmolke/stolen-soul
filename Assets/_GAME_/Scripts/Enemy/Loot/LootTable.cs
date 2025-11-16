using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new LootTable", menuName = "Enemy/New LootTable")]
public class LootTable : ScriptableObject
{
    [Header("Possible Loot")]
    public List<ItemBase> items;

    public ItemBase GetLoot()
    {
        int roll = Random.Range(1, 101);
        ItemBase best = null;

        foreach (var item in items)
        {
            if (roll <= item.dropChance)
            {
                if (best == null || item.dropChance < best.dropChance)
                    best = item;
            }
        }

        return best;
    }
}
