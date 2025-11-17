using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    [Header("Enemy Prefab")]
    public GameObject EnemyPrefab;

    [Header("Enemies to Spawn")]
    public List<EnemySpawnEntry> Enemies;

    void Start()
    {
        foreach(Transform spawnPoint in transform)
        {
            if(spawnPoint.CompareTag("EnemySpawnPoint"))
                SpawnEnemyAt(spawnPoint);
        }
    }

    private void SpawnEnemyAt(Transform spawnPoint)
    {
        EnemyData enemyData = GetRandomEnemy();
        if (enemyData != null)
        {
            GameObject enemyInstance = Instantiate(EnemyPrefab, spawnPoint.position, spawnPoint.rotation);
            EnemyAI enemyComponent = enemyInstance.GetComponent<EnemyAI>();
            if (enemyComponent != null)
            {
                enemyComponent.Init(enemyData);
            }
        }
    }

    private EnemyData GetRandomEnemy()
    {
        int totalChance = 0;
        foreach (var entry in Enemies)
        {
            totalChance += entry.spawnChance;
        }
        int randomValue = Random.Range(0, totalChance);
        int cumulativeChance = 0;
        foreach (var entry in Enemies)
        {
            cumulativeChance += entry.spawnChance;
            if (randomValue < cumulativeChance)
            {
                return entry.enemyData;
            }
        }
        return null;
    }
}
