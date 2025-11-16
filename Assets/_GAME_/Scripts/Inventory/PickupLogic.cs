using System.Collections;
using UnityEngine;

public class PickUpLogic : MonoBehaviour
{
    public ItemBase item;

    private InventoryManager1 inventoryManager;

    IEnumerator Start()
    {
        // InventoryManager initialization
        while (InventoryManager1.Instance == null)
            yield return null;

        inventoryManager = InventoryManager1.Instance;

        transform.GetComponent<SpriteRenderer>().sprite = item.itemSprite;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (inventoryManager != null)
            {
                bool wasAdded = inventoryManager.AddItem(item, 1);
                if(wasAdded) Destroy(gameObject);
            }
            else
            {
                Debug.LogError("❌ inventoryManager == null on collision!");
            }
        }
    }
}
