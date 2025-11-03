using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private int quantity;
    [SerializeField] private Sprite sprite;

    private InventoryManager inventoryManager;

    IEnumerator Start()
    {
        // InventoryManager initialization
        while (InventoryManager.Instance == null)
            yield return null;

        inventoryManager = InventoryManager.Instance;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (inventoryManager != null)
            {
                inventoryManager.AddItem(itemName, quantity, sprite);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("❌ inventoryManager == null on collision!");
            }
        }
    }
}
