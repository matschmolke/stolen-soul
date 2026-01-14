using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    public string spawnId;

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(spawnId))
        {
            spawnId = gameObject.name;
        }
    }
}
