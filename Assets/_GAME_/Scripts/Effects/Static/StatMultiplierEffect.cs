using UnityEngine;

public enum MultiplierType
{
    AttackDamage,
    SpellEffect
}

[CreateAssetMenu(fileName = "New StatMultiplier Effect", menuName = "Effects/New StatMultiplier Effect")]
public class StatMultiplierEffect : StaticEffect
{
    [Tooltip("Percent bonus, e.g. 20 = +20%")]
    public float percentageAmount;

    public MultiplierType multiplierType;

    private float multiplier => 1f + (percentageAmount / 100f);

    public override void Apply(PlayerStats1 stats)
    {
        switch (multiplierType)
        {
            case MultiplierType.AttackDamage:
                stats.attackDamageMultiplier *= multiplier;
                break;
            case MultiplierType.SpellEffect:
                stats.spellEffectMultiplier *= multiplier;
                break;
        }
    }

    public override void Remove(PlayerStats1 stats)
    {
        switch (multiplierType)
        {
            case MultiplierType.AttackDamage:
                stats.attackDamageMultiplier /= multiplier;
                break;
            case MultiplierType.SpellEffect:
                stats.spellEffectMultiplier /= multiplier;
                break;
        }
    }

    protected override void GenerateDescription()
    {
        switch (multiplierType)
        {
            case MultiplierType.AttackDamage:
                description = $"Increases Attack Damage by {((multiplier - 1f) * 100f):0}%";
                break;
            case MultiplierType.SpellEffect:
                description = $"Increases Spell Power by {((multiplier - 1f) * 100f):0}%";
                break;
        }
    }
}
