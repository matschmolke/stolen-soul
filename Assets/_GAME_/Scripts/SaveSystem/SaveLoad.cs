using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoad : MonoBehaviour
{
    public static bool PendingLoad { get; private set; }

    public static void StartContinue()
    {
        PendingLoad = true;

        DontDestroyOnLoadCleaner.Clear();

        SceneLoader.StartStatic();
        SceneManager.LoadScene(GameState.LoadedData.sceneName);
        SceneManager.LoadSceneAsync("mod1", LoadSceneMode.Additive);
    }

    public static void ApplyLoadedGame()
    {
        if (!PendingLoad || !GameState.RestoreFromSave)
            return;

        var data = GameState.LoadedData;
        if (data == null) return;

        var player = Movements.Instance;
        var stats = PlayerStats.Instance;

        if (player == null || stats == null)
            return;

        PendingLoad = false;

        // position
        player.transform.position = new Vector3(
            data.posX, data.posY, data.posZ
        );

        // stats
        stats.ApplyLoadedStats(data.health, data.mana);

        // Enemies
        //RestoreEnemies.Cache(data.enemies);

        Debug.Log("Save game applied");
    }


    public static void SaveGame()
    {
        SaveSystem.SavePlayer();
    }

}
