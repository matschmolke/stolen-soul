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

    public List<SceneEnemyData> enemiesPerScene = new();
    public List<string> clearedEnemySpawns = new();

    public int ProgressIndex;
    public List<Spell> learnedSpells = new();

    public ItemBase equippedArmor;
    public ItemBase equippedWeapon;
}
