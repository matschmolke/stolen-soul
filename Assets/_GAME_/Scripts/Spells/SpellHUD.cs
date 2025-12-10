using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Object = System.Object;
using System.Linq;
using System.Collections.Generic;

public class SpellHUD : MonoBehaviour
{
    public SpellCaster spellCaster;
    public SpellSlotUI slotPrefab;
    public Transform slotsContainer;

    private readonly List<SpellSlotUI> slotInstances = new();
    
    void Start()
    {
        if (spellCaster == null)
            spellCaster = FindFirstObjectByType<SpellCaster>();

        if (spellCaster == null)
        {
            Debug.LogError("SpellCaster (player) not found!");
            return;
        }

        RefreshSlots();
        spellCaster.OnSpellsChanged += RefreshSlots;
    }

    private void Update()
    {
        UpdateCooldowns();
    }
    void RefreshSlots()
    {
        foreach (var s in slotInstances)
            Destroy(s.gameObject);

        slotInstances.Clear();

        if (spellCaster.spells.Count == 0)
        {
            slotsContainer.gameObject.SetActive(false);
            return;
        }

        slotsContainer.gameObject.SetActive(true);

        for (int i = 0; i < spellCaster.spells.Count; i++)
        {
            SpellSlotUI slot = Instantiate(slotPrefab, slotsContainer);
            Debug.Log("Adding spell slot for " + spellCaster.spells[i].spellName);

            slot.SetSpell(spellCaster.spells[i], i + 1);
            Debug.Log("Set spell slot for " + spellCaster.spells[i].spellName);

            slotInstances.Add(slot);

            Debug.Log(
                "Slot: " + slot.name +
                " scale=" + slot.transform.localScale +
                " icon=" + (slot.icon != null) +
                " key=" + (slot.keyText != null) +
                " overlay=" + (slot.cooldownOverlay != null)
            );
        }
    }
    void UpdateCooldowns()
    {
        for (int i = 0; i < slotInstances.Count; i++)
        {
            float fill = spellCaster.GetCooldownFill(i);
            slotInstances[i].SetCooldown(fill);
        }
    }

    void CreateSlotUI()
    {
        GameObject slotObj;

        slotObj = Instantiate(new GameObject("SpellSlotUI"), transform);
        slotObj.AddComponent<RectTransform>();
        slotObj.AddComponent<SpellSlotUI>();
    }
}
