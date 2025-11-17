using UnityEngine;

[System.Serializable]
public class EnemySpawnEntry
{
    public EnemyData enemyData;
    [Range(0, 100)]
    public int spawnChance;
}
