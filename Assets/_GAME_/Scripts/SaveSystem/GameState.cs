using UnityEngine;

public static class GameState 
{
    public static bool RestoreFromSave { get; private set; }
    public static PlayerData LoadedData { get; private set; }

    public static bool TryLoadGame()
    {
        LoadedData = SaveSystem.LoadPlayer();

        RestoreFromSave = LoadedData != null;
        return RestoreFromSave;
    }

    public static void Clear()
    {
        RestoreFromSave = false;
        LoadedData = null;
    }
}
