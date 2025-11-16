using UnityEngine;
using UnityEditor;

public enum ResourceType
{
    Health,
    Mana,
}

[CreateAssetMenu(fileName = "New Consumable Item", menuName = "Items/New Consumable Item")]
public class Consumable : ItemBase
{
    [Header("Consumable Properties")]
    public int restoreAmount;
    public ResourceType resourceType;

    public void Awake()
    {
        Id = GenerateId();
        itemType = ItemType.Consumable;
    }

    protected override void GenerateDescription()
    {
        switch (resourceType) 
        {
            case ResourceType.Health:
                description = $"Restores {restoreAmount} of Health";
                break;
            case ResourceType.Mana:
                description = $"Restores {restoreAmount} of Mana";
                break;
            default:
                description = "Strange item. Nothing is known about it";
                break;
        }
    }
}
