using System;
using System.Collections.Generic;
using UnityEngine;

public static class LocationManager
{
    private static int locationIndex = 0;

    public static int LocationIndex 
    {
        get {  return locationIndex; }
        set
        {
            if (value < 0 && value <= locations.Count) locationIndex = value;
        }
    }

    private static readonly List<string> locations = new()
    {
        "TutorialDung",
        "ForestDung",
        "SwampDung",
        "FinalBossDung"
    };

    public static string GetCurrentLocation()
    {
        return locations[locationIndex];
    }

    public static string GetCurrentLocationName()
    {
        return locations[locationIndex].Substring(0, locations[locationIndex].Length - 4); // removes Dung
    }

    public static bool IncrementLocationIndex()
    {
        if (locationIndex < locations.Count)
        {
            locationIndex++;
            return true;
        }
        return false;
    }
}
