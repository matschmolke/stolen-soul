using System.Collections;
using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitUntil(() =>
            Movements.Instance != null &&
            PlayerStats.Instance != null
        );

        SaveLoad.ApplyLoadedGame();
    }
}
