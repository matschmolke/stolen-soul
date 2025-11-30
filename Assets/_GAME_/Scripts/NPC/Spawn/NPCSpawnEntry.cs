using UnityEngine;

[System.Serializable]
public class NPCSpawnEntry
{
    public NPCData npcData;
    [Range(0, 100)]
    public int spawnChance;
}
