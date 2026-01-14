using UnityEngine;

public static class EnemyProgress
{
    public static void MarkSpawnCleared(string spawnId)
    {
        if (string.IsNullOrEmpty(spawnId))
            return;

        var data = GameState.LoadedData;
        if (data == null)
            return;

        if (!data.clearedEnemySpawns.Contains(spawnId))
        {
            data.clearedEnemySpawns.Add(spawnId);
            Debug.Log($"Spawn cleared: {spawnId}");
        }
    }

    public static bool IsSpawnCleared(string spawnId)
    {
        var data = GameState.LoadedData;
        if (data == null)
            return false;

        return data.clearedEnemySpawns.Contains(spawnId);
    }
}
