using System;
using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public float posX;
    public float posY;
    public float posZ;

    public int health;
    public int mana;

    public string sceneName;

    public List<InventoryItemData> inventoryItems = new List<InventoryItemData>();
    public List<ChestData> chests = new();

    public List<EnemySaveData> enemies = new();
    public List<string> clearedEnemySpawns = new();

}
