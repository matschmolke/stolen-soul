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
        if (GameState.RestoreFromSave && GameState.LoadedData != null)
        {
            RestoreEnemies.Cache(GameState.LoadedData.enemies);
        }

        foreach (Transform spawnPoint in transform)
        {
            if(spawnPoint.CompareTag("EnemySpawnPoint"))
                SpawnEnemyAt(spawnPoint);
        }
    }

    private void SpawnEnemyAt(Transform spawnPoint)
    {
        Debug.Log($"Spawning enemy");
         
        EnemyData enemyData = GetRandomEnemy();
        if (enemyData == null)
            return;

        //check for saved enemy state
        if (GameState.RestoreFromSave &&
            RestoreEnemies.TryGetEnemy(enemyData.characterName, out var savedEnemy))
        {
            Debug.Log($"Restoring saved enemy: {enemyData.characterName}");
            //do not respawn dead enemies
            if (savedEnemy.health <= 0)
                return;

            GameObject enemyInstance = Instantiate(
                EnemyPrefab,
                savedEnemy.position,
                spawnPoint.rotation
            );

            EnemyAI enemy = enemyInstance.GetComponent<EnemyAI>();
            enemy.Init(enemyData);
            enemy.currentHealth = savedEnemy.health;

            return;
        }

        Debug.Log($"Default Spawning enemy");
        //default spawn
        GameObject instance = Instantiate(
            EnemyPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        EnemyAI component = instance.GetComponent<EnemyAI>();
        component.Init(enemyData);

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
