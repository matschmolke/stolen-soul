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
        for (int i = slotInstances.Count - 1; i >= 0; i--)
        {
            if (slotInstances[i] != null)
                Destroy(slotInstances[i].gameObject);
        }

        slotInstances.Clear();

        if (spellCaster == null || spellCaster.spells.Count == 0)
        {
            slotsContainer.gameObject.SetActive(false);
            return;
        }

        slotsContainer.gameObject.SetActive(true);

        for (int i = 0; i < spellCaster.spells.Count; i++)
        {
            SpellSlotUI slot = Instantiate(slotPrefab, slotsContainer);
            slot.SetSpell(spellCaster.spells[i], i + 1);
            slotInstances.Add(slot);
        }
    }
    void UpdateCooldowns()
    {
        for (int i = 0; i < slotInstances.Count; i++)
        {
            if (slotInstances[i] == null)
                continue;

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

    private void OnDestroy()
    {
        if (spellCaster != null)
            spellCaster.OnSpellsChanged -= RefreshSlots;
    }
}
