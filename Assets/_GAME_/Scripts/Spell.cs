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
}
