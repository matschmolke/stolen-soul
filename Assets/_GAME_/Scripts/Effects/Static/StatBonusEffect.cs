using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "New StatBonus Effect", menuName = "Effects/New StatBonus Effect")]
public class StatBonusEffect : StaticEffect
{
    public int bonusAmount;

    public ResourceType statType;

    public override void Apply(PlayerStats stats)
    {
        switch (statType) 
        {
            case ResourceType.Health:
                stats.MaxHealth += bonusAmount;
                break;
            case ResourceType.Mana:
                stats.MaxMana += bonusAmount;
                break;
        }
    }

    public override void Remove(PlayerStats stats)
    {
        switch (statType)
        {
            case ResourceType.Health:
                stats.MaxHealth -= bonusAmount;
                break;
            case ResourceType.Mana:
                stats.MaxMana -= bonusAmount;
                break;
        }
    }

    protected override void GenerateDescription()
    {
        switch (statType)
        {
            case ResourceType.Health:
                description = $"Grants +{bonusAmount} Max Health";
                break;
            case ResourceType.Mana:
                description = $"Grants +{bonusAmount} Max Mana";
                break;
        }
    }
}
