using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    public Spell[] spells;
    private float[] cooldownTimers;
    private Camera mainCam;

    void Start()
    {
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
            if (cooldownTimers[i] > 0) cooldownTimers[i] -= Time.deltaTime;
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
        Spell spell = spells[index];
        Vector3 mouseWorld = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;

        if (spell.spellPrefab != null)
        {
            GameObject proj = Instantiate(spell.spellPrefab, transform.position, Quaternion.identity);
            proj.GetComponent<Projectile>().Shoot(mouseWorld);
        }

        cooldownTimers[index] = spell.cooldown;
    }
}