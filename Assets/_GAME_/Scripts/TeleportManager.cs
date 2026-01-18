using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeleportManager : MonoBehaviour
{
    public GameObject Sign;

    private Text signText;
    private SceneTeleport teleport;

    private void Awake()
    {
        signText = Sign.GetComponent<Text>();
        teleport = GetComponent<SceneTeleport>();
    }

    public void Start()
    {
        teleport.targetScene = LocationManager.GetCurrentLocation();
        signText.text = LocationManager.GetCurrentLocationName();
    }
}
