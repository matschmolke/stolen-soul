using UnityEngine;

[CreateAssetMenu(menuName = "NPC/NPC Data")]
public class NPCData : CharacterData
{
    [Header("NPC")]
    public float talkRange = 1.5f;

    [Header("Trader")]
    public bool isTrader = false;
    public TraderInventoryData DefaultTraderInventory;
}