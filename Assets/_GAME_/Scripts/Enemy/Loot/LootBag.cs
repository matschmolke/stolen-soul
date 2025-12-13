using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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
        ItemBase droppedItem = loot.GetLoot();

        if (droppedItem == null)
        {
            Debug.Log("No item dropped.");
            return;
        }

        Debug.Log("Dropped: " + droppedItem.itemName);

        GameObject lootObject = Instantiate(lootPrefab, position, Quaternion.identity);

        SpriteRenderer sr = lootObject.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingOrder = 1;
        }

        lootObject.GetComponent<PickUpLogic>().item = droppedItem; 
    }
}
