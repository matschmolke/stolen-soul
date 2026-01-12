using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new LootTable", menuName = "Enemy/New LootTable")]
public class LootTable : ScriptableObject
{
    [Header("Possible Loot")]
    public List<ItemBase> items;

    [Header("Drop Settings")]
    [Tooltip("How many different item types can drop from this enemy")]
    public int maxDrops = 4; // max one per ItemType

    public List<ItemBase> GetLoot()
    {
        List<ItemBase> result = new List<ItemBase>();
        HashSet<ItemType> droppedTypes = new HashSet<ItemType>();

        List<ItemBase> shuffled = new List<ItemBase>(items);
        Shuffle(shuffled);

        foreach (var item in shuffled)
        {
            if (result.Count >= maxDrops)
                break;

            if (droppedTypes.Contains(item.itemType))
                continue; // already dropped this type

            int roll = Random.Range(1, 101);

            if (roll <= item.dropChance)
            {
                result.Add(item);
                droppedTypes.Add(item.itemType);
            }
        }

        return result;
    }

    void Shuffle(List<ItemBase> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rnd = Random.Range(i, list.Count);
            (list[i], list[rnd]) = (list[rnd], list[i]);
        }
    }
}
