using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoad : MonoBehaviour
{
    //load save flag
    public static bool PendingLoad = false;
    public static bool restoreInventory = false;

    private static PlayerData cachedData;

    public static List<InventoryItemData> GetSavedInventory()
    {
        return cachedData?.inventoryItems;
    }

    public static void SaveGame()
    {
        SaveSystem.SavePlayer();
    }

    public static void ContinueGame()
    {
        cachedData = SaveSystem.LoadPlayer();
        if (cachedData == null)
        {
            Debug.LogWarning("No save file to load");
            return;
        }

        PendingLoad = true;

        DontDestroyOnLoadCleaner.Clear();

        SceneLoader.StartStatic();
        SceneManager.LoadScene(cachedData.sceneName);
        SceneManager.LoadSceneAsync("mod1", LoadSceneMode.Additive);

    }


    public static void ApplyLoadedGame()
    {
        if (!PendingLoad || cachedData == null)
            return;

        PendingLoad = false;
        restoreInventory = true;

        var player = Movements.Instance;
        var stats = PlayerStats.Instance;

        if (player == null || stats == null)
        {
            Debug.LogWarning("ApplyLoadedGame waiting for player/stats");
            return;
        }

        //player position
        player.transform.position = new Vector3(
            cachedData.posX,
            cachedData.posY,
            cachedData.posZ
        );

        //player stats
        stats.ApplyLoadedStats(
            cachedData.health,
            cachedData.mana
        );

        //chests
        RestoreSavedChests.Restore(cachedData.chests);

        Debug.Log("Save game applied successfully");
    }

}
