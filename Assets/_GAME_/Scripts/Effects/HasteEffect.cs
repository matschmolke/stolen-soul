using UnityEngine;
using System.Collections;

public static class HasteEffect
{
    public static void Apply(GameObject target, float duration)
    {
        Debug.Log("HasteEffect: Apply");

        float currentWalkSpeed = PlayerStats.Instance.playerScript.GetWalkSpeed();
        float currentRunSpeed = PlayerStats.Instance.playerScript.GetRunSpeed();
        float multiplier = 1.5f;

        PlayerStats.Instance.playerScript.SetWalkSpeed(currentWalkSpeed * multiplier);
        PlayerStats.Instance.playerScript.SetRunSpeed(currentRunSpeed * multiplier);

        target.GetComponent<MonoBehaviour>().StartCoroutine(RemoveHaste(duration, currentWalkSpeed, currentRunSpeed));
    }

    private static IEnumerator RemoveHaste(float duration, float originalWalkSpeed, float originalRunSpeed)
    {
        yield return new WaitForSeconds(duration);

        PlayerStats.Instance.playerScript.SetWalkSpeed(originalWalkSpeed);
        PlayerStats.Instance.playerScript.SetRunSpeed(originalRunSpeed);

        Debug.Log("Haste ended!");
    }
}
