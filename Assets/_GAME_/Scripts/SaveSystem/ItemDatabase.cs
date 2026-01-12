using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance;

    public List<ItemBase> items;

    private Dictionary<string, ItemBase> itemsByName;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        itemsByName = new Dictionary<string, ItemBase>();

        foreach (var item in items)
        {
            if (!itemsByName.ContainsKey(item.itemName))
                itemsByName.Add(item.itemName, item);
            else
                Debug.LogWarning($"Duplicate itemName in ItemDatabase: {item.itemName}");
        }
    }

    public ItemBase GetItemByName(string name)
    {
        itemsByName.TryGetValue(name, out var item);
        return item;
    }
}
