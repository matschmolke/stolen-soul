using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class RestoreEnemies
{
    private static Dictionary<string, List<EnemySaveData>> cachedEnemies;

    public static void Cache(PlayerData data)
    {
        cachedEnemies = new Dictionary<string, List<EnemySaveData>>();

        if (data == null)
            return;

        string scene = SceneManager.GetActiveScene().name;

        var sceneData = data.enemiesPerScene
            .Find(e => e.sceneName == scene);

        if (sceneData == null)
            return;

        foreach (var enemy in sceneData.enemies)
        {
            if (!cachedEnemies.TryGetValue(enemy.enemyName, out var list))
            {
                list = new List<EnemySaveData>();
                cachedEnemies.Add(enemy.enemyName, list);
            }

            list.Add(enemy);
        }
    }


    public static bool TryGetEnemy(string enemyName, out EnemySaveData data)
    {
        data = null;

        if (cachedEnemies == null)
        {
            Debug.LogWarning("No cached enemies available for restoration");
            return false;
        }

        if (!cachedEnemies.TryGetValue(enemyName, out var list))
        {
            Debug.LogWarning($"No cached enemies found with name {enemyName}");
            return false;
        }

        if (list.Count == 0)
            return false;

        // fix order issue by always taking the first one
        data = list[0];
        list.RemoveAt(0);

        return true;
    }

}
