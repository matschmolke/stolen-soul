using UnityEngine;
using System.Collections.Generic;

public class LootBag : MonoBehaviour
{
    public GameObject lootPrefab;
    private LootTable loot;

    public void SetLoot(LootTable lootTable)
    {
        loot = lootTable;
    }

    public void DropLoot(Vector2 position)
    {
        List<ItemBase> droppedItems = loot.GetLoot();

        if (droppedItems.Count == 0)
        {
            Debug.Log("No loot dropped.");
            return;
        }

        float spread = 0.5f;

        for (int i = 0; i < droppedItems.Count; i++)
        {
            ItemBase item = droppedItems[i];

            Vector2 offset = Random.insideUnitCircle * spread;
            GameObject lootObject = Instantiate(lootPrefab, position + offset, Quaternion.identity);

            SpriteRenderer sr = lootObject.GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.sortingOrder = 1;

            lootObject.GetComponent<PickUpLogic>().item = item;

            Debug.Log("Dropped: " + item.itemName);
        }
    }
}
