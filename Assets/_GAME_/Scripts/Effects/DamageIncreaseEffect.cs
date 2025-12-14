using UnityEngine;
using System.Collections;

public static class DamageIncreaseEffect
{
    public static void Apply(GameObject target, float duration)
    {
        Debug.Log("DamageIncreaseEffect: Apply");

        int originalAttackDamage = PlayerStats.Instance.Attack;
        float multiplier = 1.5f;

        PlayerStats.Instance.Attack = 
            (int)(PlayerStats.Instance.Attack * multiplier);
        
        target.GetComponent<MonoBehaviour>().StartCoroutine(RemoveDamageIncrease(duration, originalAttackDamage));
    }

    private static IEnumerator RemoveDamageIncrease(float duration, int originalAttackDamage)
    {
        yield return new WaitForSeconds(duration);

        PlayerStats.Instance.Attack = originalAttackDamage;

        Debug.Log("Damage Increase ended!");
    }
}
