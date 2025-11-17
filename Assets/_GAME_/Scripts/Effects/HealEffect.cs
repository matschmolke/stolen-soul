using UnityEngine;

public static class HealEffect
{
    public static void Apply(int amount)
    {
        Debug.Log("HealEffect: Apply");
        
        if (PlayerStats.Instance.CurrentHealthScore >= PlayerStats.Instance.maxScore) return;
        PlayerStats.Instance.Heal(amount);
        Debug.Log($"Healed {amount} HP!");
    }
}