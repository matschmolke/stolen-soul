using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D.Animation;

[Flags]
public enum EquipmentType
{
    Weapon = 1 << 0,
    Armor = 1 << 1,
}

[CreateAssetMenu(fileName = "New Equipment Item", menuName = "Items/New Equipment Item")]
public class Equipment : ItemBase
{
    [Header("Equipment Type")]
    public EquipmentType equipmentType;

    [Header("Equipment Properties")]
    public int attackDmg;
    public int armorValue;

    [Header("Special Effects")]
    public StaticEffect[] effects = null;

    [Header("Sprites for Armor")]
    public SpriteLibraryAsset armorSpriteLibrary;

    public void Awake()
    {
        Id = GenerateId();
        itemType = ItemType.Equipment;
    }

    protected override void GenerateDescription()
    {
        switch (equipmentType)
        {
            case EquipmentType.Weapon:
                description = $"Grants <b>+{attackDmg}</b> attack damage.";
                break;
            case EquipmentType.Armor:
                description = $"Negates <b>+{armorValue}</b> damage.";
                break;
            default:
                description = "Strange item. Nothing is known about it";
                break;
        }

        if (effects != null)
        {
            foreach (var effect in effects)
            {
                description += "\n" + effect.description;
            }
        }
    }
}
