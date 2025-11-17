using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager Instance;
    
    public List<Effect> activeEffects = new();
    public EffectSlotUI[] slots;

    private void Awake()
    {
        Instance = this;
        UpdateUI();
    }

    public void AddEffect(Effect effect)
    {
        Debug.Log("EffectsManager void AddEffect");
        
        if (activeEffects.Contains(effect)) return;
        
        Debug.Log("Slots Length: " + slots.Length);
        
        if (activeEffects.Count >= slots.Length)
        {
            Debug.Log("No free effect slots!");
            Debug.Log(activeEffects.Count);
            
            foreach(var  e in activeEffects)
                Debug.Log(e.effectName);
            
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
        Debug.Log("EffectsManager UpdateUI!!!!!");
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetEffect(i < activeEffects.Count ? activeEffects[i] : null);
        }
    }
}
