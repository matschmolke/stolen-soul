using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager Instance;
    
    public List<Effect> activeEffects = new();
    public EffectSlotUI[] slots;

    private void Awake() => Instance = this;

    public void AddEffect(Effect effect)
    {
        Debug.Log("EffectsManager void AddEffect");
        
        if (activeEffects.Contains(effect)) return;
        
        if (activeEffects.Count >= slots.Length)
        {
            Debug.Log("No free effect slots!");
            return;
        }
        
        activeEffects.Add(effect);
        
        Debug.Log("Added effect " + effect.effectName);
        
        UpdateUI();
    }

    public void RemoveEffect(Effect effect)
    {
        if (!activeEffects.Contains(effect)) return;
        
        activeEffects.Remove(effect);
        UpdateUI();
    }

    private void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetEffect(i < activeEffects.Count ? activeEffects[i] : null);
        }
    }
}
