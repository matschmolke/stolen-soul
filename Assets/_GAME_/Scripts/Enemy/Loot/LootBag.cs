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

        if(droppedItem == null)
        {
            Debug.Log("No item dropped.");
            return;
        }

        GameObject lootObject = Instantiate(lootPrefab, position, Quaternion.identity);
        lootObject.GetComponent<PickUpLogic>().item = droppedItem; 
    }
}
