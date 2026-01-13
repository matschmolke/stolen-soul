using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void Start()
    {
        StartStatic();
    }

    public static void StartStatic()
    {
        if (Movements.Instance == null)
        {
            SceneManager.LoadSceneAsync("PlayerScene", LoadSceneMode.Additive);
        }
        else
        {
            Debug.Log("Player already exists - do not load PlayerScene");
        }
        SceneManager.LoadSceneAsync("TutorialDung", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("InventoryScene", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("mod1", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("NPCScene", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("TradeScene", LoadSceneMode.Additive);
    }
}
