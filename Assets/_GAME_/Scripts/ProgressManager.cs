using System;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager Instance;

    private int locationIndex = 0;

    public int LocationIndex
    {
        get { return locationIndex; }
        set
        {
            if (value >= 0 && value < locations.Count) locationIndex = value;
        }
    }

    private readonly List<string> locations = new()
    {
        "TutorialDung",
        "ForestDung",
        "SwampDung",
        "FinalBossDung"
    };

    private void Awake()
    {
        if (GameState.RestoreFromSave)
        {
            LocationIndex = GameState.LoadedData.ProgressIndex;
        }

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public string GetCurrentLocation()
    {
        return locations[locationIndex];
    }

    public string GetCurrentLocationName()
    {
        return locations[locationIndex].Substring(0, locations[locationIndex].Length - 4); // removes Dung
    }
}
