using System.Collections;
using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    public Spell[] spells;
    private float[] cooldownTimers;
    private Camera mainCam;

    void Start()
    {
        if (spells == null || spells.Length == 0)
        {
            Debug.LogError("No spells assigned in SpellCaster!");
            return;
        }

        cooldownTimers = new float[spells.Length];
        mainCam = Camera.main;
    }

    void Update()
    {
        HandleCooldowns();
        HandleInput();
    }

    void HandleCooldowns()
    {
        for (int i = 0; i < cooldownTimers.Length; i++)
        {
            if (cooldownTimers[i] > 0) cooldownTimers[i] -= Time.deltaTime;
        }
    }

    void HandleInput()
    {
        for (int i = 0; i < spells.Length; i++)
        {
            if (cooldownTimers[i] > 0) continue;
            if (Input.GetKeyDown(spells[i].castKey))
            {
                Cast(i);
            }
        }
    }

    void Cast(int index)
    {
        if (spells == null || spells.Length <= index || spells[index] == null) return;

        Spell spell = spells[index];

        cooldownTimers[index] = spell.cooldown;

        if (spell.isProjectile && spell.spellPrefab != null)
        {
            GameObject proj = Instantiate(spell.spellPrefab, transform.position, Quaternion.identity);
            Projectile p = proj.GetComponent<Projectile>();
            if (p != null)
            {
                Vector3 mouseWorld = mainCam.ScreenToWorldPoint(Input.mousePosition);
                mouseWorld.z = 0;
                p.Shoot(mouseWorld);
            }
            else
            {
                Debug.LogWarning($"Prefab {spell.spellName} не має компонента Projectile!");
            }
        }
        else if (!spell.isProjectile)
        {
            if (spell.spellName == "Invisibility")
            {
                StartCoroutine(ActivateInvisibility(spell.cooldown / 2));
            }
            else if (spell.spellName == "Heal")
            {
                Heal(spell.manaCost);
            }
        }
    }

    IEnumerator ActivateInvisibility(float duration)
    {
        Debug.Log("Invisibility activated!");
        yield return new WaitForSeconds(duration);
        Debug.Log("Invisibility ended!");
    }

    void Heal(float amount)
    {
        Debug.Log($"Heal {amount} HP!");
    }
}
