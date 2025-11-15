using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellHUD : MonoBehaviour
{
    public SpellCaster spellCaster;
    public SpellSlotUI[] slots;
    void Start()
    {
        if (spellCaster == null)
        {
            spellCaster = FindObjectOfType<SpellCaster>();
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
}
