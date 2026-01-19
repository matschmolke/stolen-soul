using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SaveSystem
{
    private static string SavePath =>
        Path.Combine(Application.persistentDataPath, "save.json");

    public static void SavePlayer()
    {
        var player = Movements.Instance;
        var stats = PlayerStats.Instance;
        var inventory = PlayerInventory.Instance;
        var spellCaster = SpellCaster.Instance;

        if (player == null || stats == null || inventory == null || spellCaster == null)
        {
            Debug.LogError("Save failed: Player, PlayerStats, or Inventory is null");
            return;
        }

        PlayerData data = new PlayerData
        {
            posX = player.transform.position.x,
            posY = player.transform.position.y,
            posZ = player.transform.position.z,
            health = stats.currentHealth,
            mana = stats.currentMana,
            sceneName = SceneManager.GetActiveScene().name,

            clearedEnemySpawns = new List<string>(
            GameState.LoadedData?.clearedEnemySpawns ?? new List<string>()),

            ProgressIndex = ProgressManager.Instance.LocationIndex,

            learnedSpells = spellCaster.spells,
        };

        // Inventory
        foreach (var slot in inventory.slots)
        {
            if (slot.Value.Item == null) continue;

            data.inventoryItems.Add(new InventoryItemData(
                slot.Value.Item.itemName,
                slot.Value.Quantity,
                slot.Key
            ));

            if (slot.Key == 24 && slot.Value.Item != null) data.equippedWeapon = slot.Value.Item;
            if (slot.Key == 25 && slot.Value.Item != null) data.equippedArmor = slot.Value.Item;
        }

        // Chests
        data.chests = SaveChests();

        // Enemies
        //data.enemies = SaveEnemies();
        
        SaveEnemiesForScene(data);


        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);

        Debug.Log($"Game saved to {SavePath}");
    }

    public static PlayerData LoadPlayer()
    {
        if (!File.Exists(SavePath))
        {
            Debug.LogWarning("No save file found");
            return null;
        }

        string json = File.ReadAllText(SavePath);
        PlayerData data = JsonUtility.FromJson<PlayerData>(json);

        Debug.Log("Game loaded from save file");
        return data;
    }

    public static List<ChestData> SaveChests()
    {
        var result = new List<ChestData>();

        foreach (var chest in ChestInventory.AllChests.Values)
        {
            ChestData data = new ChestData
            {
                chestId = chest.chestId
            };

            foreach (var slot in chest.slots)
            {
                var invItem = slot.Value;
                if (invItem.Item == null) continue;

                data.items.Add(new InventoryItemData(
                    invItem.Item.itemName,
                    invItem.Quantity,
                    slot.Key
                ));
            }

            result.Add(data);
        }

        return result;
    }

    public static List<EnemySaveData> SaveEnemies()
    {
        var result = new List<EnemySaveData>();
        string currentScene = SceneManager.GetActiveScene().name;

        foreach (var enemy in Object.FindObjectsOfType<EnemyAI>())
        {
            if (enemy.isDead)
                continue;

            result.Add(new EnemySaveData
            {
                spawnId = enemy.spawnId,
                enemyName = enemy.Data.characterName,
                position = enemy.transform.position,
                health = enemy.currentHealth,
                sceneName = currentScene
            });
        }

        return result;
    }

    public static void SaveEnemiesForScene(PlayerData data)
    {
        string scene = SceneManager.GetActiveScene().name;

        data.enemiesPerScene.RemoveAll(e => e.sceneName == scene);

        SceneEnemyData sceneData = new SceneEnemyData
        {
            sceneName = scene
        };

        foreach (var enemy in Object.FindObjectsOfType<EnemyAI>())
        {
            if (enemy.isDead)
                continue;

            sceneData.enemies.Add(new EnemySaveData
            {
                spawnId = enemy.spawnId,
                enemyName = enemy.Data.characterName,
                position = enemy.transform.position,
                health = enemy.currentHealth,
                sceneName = scene
            });
        }

        data.enemiesPerScene.Add(sceneData);
    }

}
