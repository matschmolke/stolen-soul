using System.Collections;
using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    public Spell[] spells;
    private float[] cooldownTimers;
    private Camera mainCam;
    private Vector3 targetLocation;
    
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
        if (spells == null || spells.Length <= index || spells[index] == null) return;

        Spell spell = spells[index];
        cooldownTimers[index] = spell.cooldown;

        // 🔥 ОНОВЛЮЄМО targetLocation кожного разу
        targetLocation = mainCam.ScreenToWorldPoint(Input.mousePosition);
        targetLocation.z = transform.position.z;

        if (spell.isProjectile && spell.spellPrefab != null)
        {
            GameObject proj = Instantiate(spell.spellPrefab, transform.position, Quaternion.identity);

            // ROTATION
            Vector3 dir = targetLocation - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            if (spell.spellName == "Fireball")
                angle += 90f;
            
            proj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            
            var sr = proj.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                if (spell.spellName == "Fireball")
                {
                    // Fireball: flip тільки коли вниз
                    sr.flipX = dir.y < 0;
                }
                else if (spell.spellName == "Ice Shard")
                {
                    if (dir.y >= 0) 
                    {
                        // ВГОРУ
                        sr.flipX = true;
                        sr.flipY = false;
                    }
                    else 
                    {
                        // ВНИЗ
                        sr.flipX = true;
                        sr.flipY = true;
                    }
                }
            }
            
            // SHOOT
            Projectile p = proj.GetComponent<Projectile>();
            if (p != null)
            {
                p.Shoot(targetLocation, spell.speed);
            }
            else
            {
                Debug.LogWarning($"Prefab {spell.spellName} does not have a Projectile!");
            }
        }
        else if (!spell.isProjectile)
        {
            if (spell.spellName == "Invisibility")
                StartCoroutine(ActivateInvisibility(spell.cooldown / 2));
            else if (spell.spellName == "Heal")
                Heal(spell.manaCost);
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
