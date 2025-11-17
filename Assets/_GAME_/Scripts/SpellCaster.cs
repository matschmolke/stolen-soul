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
        if (mainCam == null)
            Debug.LogError("No MainCamera found in the scene!");
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
                Debug.Log("Spell casting!!!");
                Cast(i);
            }
        }
    }

    void Cast(int index)
    {
        Debug.Log("void CAST");
        Spell spell = spells[index];

        if (PlayerStats.Instance.currentMana < spell.manaCost) return;

        cooldownTimers[index] = spell.cooldown;
        PlayerStats.Instance.UseMana((int)spell.manaCost);

        Vector3 target = GetMouseWorld();

        if (spell.isProjectile)
            CastProjectileSpell(spell, target);
        else
            CastUtilitySpell(spell);
    }

    Vector3 GetMouseWorld()
    {
        Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;
        return mousePos;
    }

    void CastProjectileSpell(Spell spell, Vector3 target)
    {
        GameObject proj = Instantiate(spell.spellPrefab, transform.position, Quaternion.identity);

        RotateProjectile(proj, spell, target);
        FlipProjectileSprite(proj, spell, target);

        Projectile p = proj.GetComponent<Projectile>();
        if (p != null)
            p.Shoot(target, spell.speed);
    }
    void FlipProjectileSprite(GameObject proj, Spell spell, Vector3 target)
    {
        SpriteRenderer sr = proj.GetComponent<SpriteRenderer>();
        if (sr == null) return;

        Vector3 dir = target - transform.position;

        if (spell.spellName == "Fireball")
        {
            sr.flipX = dir.y < 0;
            return;
        }
        if (spell.spellName == "Ice Shard")
        {
            sr.flipX = true;     
            sr.flipY = dir.y < 0;
            return;
        }
    }
    
    void RotateProjectile(GameObject proj, Spell spell, Vector3 target)
    {
        Vector3 dir = target - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if (spell.spellName == "Fireball")
            angle += 90;

        proj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    void CastUtilitySpell(Spell spell)
    {
        Debug.Log("Cast Utility Spell " +  spell.spellName);
        if (spell.effect != null)
            EffectsManager.Instance.AddEffect(spell.effect, spell.duration);

        spell.ApplyEffect(this.gameObject);
    }

    public float GetCooldownFill(int index)
    {
        if (index < 0 || index >= cooldownTimers.Length) return 0f;
        return cooldownTimers[index] / spells[index].cooldown;
    }
}
