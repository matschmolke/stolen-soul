using UnityEngine;

public static class HealEffect
{
    public static void Apply(int amount)
    {
        Debug.Log("HealEffect: Apply");
        
        if (PlayerStats.Instance.currentHealth >= PlayerStats.Instance.MaxHealth) return;
        PlayerStats.Instance.Heal(amount);
        Debug.Log($"Healed {amount} HP!");
    }
}