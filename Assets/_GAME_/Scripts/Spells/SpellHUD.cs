using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Object = System.Object;

public class SpellHUD : MonoBehaviour
{
    public SpellCaster spellCaster;
    public SpellSlotUI[] slots;
    void Start()
    {
        if (spellCaster == null)
        {
            spellCaster = FindFirstObjectByType<SpellCaster>();
        }

        if (spellCaster == null)
        {
            Debug.LogError("SpellCaster (player) not found!");
            return;
        }
        
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetSpell(spellCaster.spells[i], i + 1);
        }
    }

    private void Update()
    {
        UpdateCooldowns();
    }

    void UpdateCooldowns()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i >= spellCaster.spells.Length) continue;

            float fill = spellCaster.GetCooldownFill(i);
            slots[i].SetCooldown(fill);
        }
    }
}
