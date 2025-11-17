using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("General")]
    public string enemyName;
    public Sprite sprite;
    public RuntimeAnimatorController animatorController;

    [Header("Stats")]
    public float maxHealth = 100f;
    public float attackDamage = 10f;
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float visionRange = 8f;
    public float attackRange = 1.5f;

    [Header("Loot")]
    public LootTable lootTable;
}
