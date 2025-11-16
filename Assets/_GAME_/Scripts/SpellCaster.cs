using System.Collections;
using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    public Spell[] spells;
    private float[] cooldownTimers;
    private Camera mainCam;
    private Vector3 targetLocation;
    private PlayerStats playerStats;
    
    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats not found on the player!");
        }
        
        if (spells == null || spells.Length == 0)
        {
            Debug.LogError("No spells assigned in SpellCaster!");
            return;
        }

        cooldownTimers = new float[spells.Length];
        mainCam = Camera.main;
        
        if (mainCam == null)
        {
            Debug.LogError("No MainCamera found in the scene!");
        }

        targetLocation = mainCam.ScreenToWorldPoint(Input.mousePosition);
        targetLocation.z = transform.position.z;
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
        if (PlayerStats.Instance.CurrentManaScore >= spells[index].manaCost)
        {
            Spell spell = spells[index];
            cooldownTimers[index] = spell.cooldown;
            
            if (spell.spellName == "Heal" && PlayerStats.Instance.CurrentHealthScore == PlayerStats.Instance.maxScore) return;
            
            PlayerStats.Instance.UseMana((int)spell.manaCost);

            Vector3 target = GetMouseWorld();

            if (spell.isProjectile)
                CastProjectileSpell(spell, target);
            else
                CastUtilitySpell(spell);   
        }
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

    void RotateProjectile(GameObject proj, Spell spell, Vector3 target)
    {
        Vector3 dir = target - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if (spell.spellName == "Fireball")
            angle += 90;

        proj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
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
    
    void CastUtilitySpell(Spell spell)
    {
        if (spell.spellName == "Invisibility")
        {
            StartCoroutine(ActivateInvisibility(spell.duration, spell.manaCost));
        }
        else if (spell.spellName == "Heal")
        {
            Heal(spell.manaCost);
        }
    }

    IEnumerator ActivateInvisibility(float duration, float amount)
    {
        Debug.Log("Invisibility activated!");
        
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
        
        Color[] originalColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            originalColors[i] = renderers[i].color;
            Color c = renderers[i].color;
            c.a = 0.5f;
            renderers[i].color = c;
        }
        yield return new WaitForSeconds(duration);
        
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].color = originalColors[i];
        }
        
        Debug.Log("Invisibility ended!");
    }

    void Heal(float amount)
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.Heal((int)amount);
            Debug.Log($"Healed {amount} HP!");
        }
    }

    public float GetCooldownFill(int index)
    {
        if (index < 0 || index >= cooldownTimers.Length) return 0f;
        Debug.Log("Cooldown fill: " + cooldownTimers[index] / spells[index].cooldown);
        return cooldownTimers[index] / spells[index].cooldown;
    }
}
