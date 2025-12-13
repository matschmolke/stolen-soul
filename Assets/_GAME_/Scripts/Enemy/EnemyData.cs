using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy Data")]
public class EnemyData : CharacterData
{
    [Header("Combat")]
    public float attackRange = 1.0f;
    public float attackDamage = 10f;

    [Header("AI")]
    public float visionRange = 5f;

    [Header("Loot")]
    public LootTable lootTable;
}