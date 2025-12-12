using UnityEngine;

[CreateAssetMenu(menuName = "NPC/NPC Data")]
public class NPCData : CharacterData
{
    [Header("NPC")]
    public float talkRange = 1.5f;

    [Header("Hostility")]
    public bool canBecomeHostile = true;
    public float hostileAttackRange = 1.2f;
    public float hostileDamage = 5f;

    [Header("Trader")]
    public bool isTrader = false;
    public TraderInventoryData DefaultTraderInventory;

    public EnemyData enemyVersion;
}