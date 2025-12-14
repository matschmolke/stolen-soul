using UnityEngine;
using System.Collections;

public static class DefenceIncreaseEffect
{
    public static void Apply(GameObject target, float duration)
    {
        Debug.Log("DefenceIncreaseEffect: Apply");

        int originalDefence = PlayerStats.Instance.Defence;
        float multiplier = 1.5f;

        PlayerStats.Instance.Defence =
            (int)(PlayerStats.Instance.Defence * multiplier);
        
        target.GetComponent<MonoBehaviour>().StartCoroutine(RemoveDefenceIncrease(duration, originalDefence));
    }

    private static IEnumerator RemoveDefenceIncrease(float duration, int originalDefence)
    {
        yield return new WaitForSeconds(duration);

        PlayerStats.Instance.Defence = originalDefence;

        Debug.Log("Defence Increase ended!");
    }
}
