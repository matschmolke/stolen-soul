using System;
using UnityEditor;
using UnityEngine;

[Flags]
public enum ItemType
{
    Consumable = 1 << 1,
    Equipment = 1 << 2,
    Scroll = 1 << 3,
    Misc = 1 << 4,
}
   
public abstract class ItemBase : ScriptableObject
{
    [Header("Info")]
    public string Id;
    public ItemType itemType;

    [Header("Display Settings")]
    public string itemName;
    public Sprite itemSprite;

    [TextArea]
    public string description;

    [Header("Stack Settings")]
    public bool isStackable;
    public int maxStackSize;

    [Header("Loot Settings")]
    public bool isUnique;
    public int dropChance;

    [Header("Trade")]
    public int value;

    protected void OnValidate()
    {
        GenerateDescription();

        if (!isStackable) maxStackSize = 1;
    }

    protected abstract void GenerateDescription();

    protected string GenerateId()
    {
        return System.Guid.NewGuid().ToString();
    }
}
