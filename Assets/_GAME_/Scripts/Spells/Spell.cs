using UnityEngine;

[CreateAssetMenu(menuName = "Spell")]
public class Spell : ScriptableObject
{
    [Header("Basic Info")]
    public string spellName;
    public Sprite icon;
    public string castKey;
    
    [Header("Cost and Cooldown")]
    public float cooldown;
    public float manaCost;
    public float duration;
    
    [Header("Projectile Info")]
    public GameObject spellPrefab;
    public float damage;
    
    [Header("Optional Effects")]
    public float speed = 10f;

    public bool isProjectile = true;
    public Effect effect;

    public void ApplyEffect(GameObject target)
    {
        switch (spellName)
        {
            case "Heal":
                HealEffect.Apply((int)manaCost);
                break;
            case "Invisibility":
                InvisibilityEffect.Apply(target, duration);
                break;
            case "Haste":
                HasteEffect.Apply(target, duration);
                break;
            case "Increased Damage":
                break;
            case "Increased Armor":
                break;

        }
    }
}

