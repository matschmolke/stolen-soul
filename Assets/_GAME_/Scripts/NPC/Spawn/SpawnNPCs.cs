using System.Collections.Generic;
using UnityEngine;

public class SpawnNPCs : MonoBehaviour
{
    [Header("NPC Prefab")]
    public GameObject NPCPrefab;

    [Header("NPCs to Spawn")]
    public List<NPCSpawnEntry> NPCs;

    void Start()
    {
        foreach(Transform spawnPoint in transform)
        {
            if(spawnPoint.CompareTag("NPCSpawnPoint"))
                SpawnNPCAt(spawnPoint);
        }
    }

    private void SpawnNPCAt(Transform spawnPoint)
    {
        NPCData npcData = GetRandomNPC();
        if (npcData != null)
        {
            GameObject npcInstance = Instantiate(NPCPrefab, spawnPoint.position, spawnPoint.rotation);
            NPCAI npcComponent = npcInstance.GetComponent<NPCAI>();
            if (npcComponent != null)
            {
                npcComponent.Data = npcData;
                npcComponent.Init(npcData);
            }
        }
    }

    private NPCData GetRandomNPC()
    {
        int totalChance = 0;
        foreach (var entry in NPCs)
        {
            totalChance += entry.spawnChance;
        }
        int randomValue = Random.Range(0, totalChance);
        int cumulativeChance = 0;
        foreach (var entry in NPCs)
        {
            cumulativeChance += entry.spawnChance;
            if (randomValue < cumulativeChance)
            {
                return entry.npcData;
            }
        }
        return null;
    }
}
