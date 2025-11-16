using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;

public class PlayerStats1 : MonoBehaviour
{
    private int maxHealth = 20;

    public int MaxHealth 
    {
        get => maxHealth;
        set 
        {
            maxHealth = value;
            OnHealthChanged?.Invoke(currentHealth, MaxHealth);
        }
    }

    private int maxMana = 20;

    public int MaxMana
    {
        get => maxMana;
        set
        {
            maxMana = value;
            OnManaChanged?.Invoke(currentMana, MaxMana);
        }
    }

    private int attack = 5;
    public int Attack
    {
        get => attack;
        set
        {
            attack = value;
            OnAttackDamageChanged?.Invoke(value);
        }
    }
    private int defence = 5;

    public int Defence
    {
        get => defence;
        set
        {
            defence = value;
            OnDefenceChanged?.Invoke(value);
        }
    }

    [SerializeField]
    private int currentHealth;
    private int currentMana;

    private Movements playerScript;

    
    public event Action<int, int> OnHealthChanged; // (current, max)
    public event Action<int, int> OnManaChanged;

    public event Action<int> OnAttackDamageChanged;
    public event Action<int> OnDefenceChanged;

    public bool canRegenerateMana = true;

    public float attackDamageMultiplier = 1f;

    public float spellEffectMultiplier = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnManaChanged?.Invoke(currentMana, maxMana);

        //OnAttackDamageChanged?.Invoke(attack);
        //OnDefenceChanged?.Invoke(defence);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Movements>();
    }

    // Skrypty napisane pod skrypty z damagem np. PlayerStats.TakeDamage
    public void SetHealth(int newHealth)
    {
        //currentHealth = Mathf.Clamp(newHealth, 0, maxHealth);
        //UpdateVisual();
    }

    public void SetMana(int newHealth)
    {
        //currentHealth = Mathf.Clamp(newHealth, 0, maxHealth);
        //UpdateVisual();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth) currentHealth = maxHealth;

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;

        if (currentHealth <= 0) KillPlayer();

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void UseMana(int spellCost)
    {
        currentMana -= spellCost;

        if (currentMana < 0) currentMana = 0;

        OnManaChanged?.Invoke(currentMana, maxMana);
    }

    public void RestoreMana(int amount)
    {
        if (!canRegenerateMana) return;

        if (amount <= 0) return;
        currentMana += amount;

        if (currentMana > maxMana) currentMana = maxMana;

        OnManaChanged?.Invoke(currentMana, maxMana);
    }

    public void RefillAll()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnManaChanged?.Invoke(currentMana, maxMana);
    }

    public void KillPlayer()
    {
        playerScript.Dead();
        currentHealth = 0;
        currentMana = 0;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnManaChanged?.Invoke(currentMana, maxMana);
    }

    // Update is called once per frame
    void Update()
    {
        // Temporary
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            TakeDamage(1);
        }
        else if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            RefillAll();
        }
        else if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            Heal(1);
        }
        else if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            UseMana(1);
        }
        else if (Keyboard.current.zKey.wasPressedThisFrame)
        {
            KillPlayer();
        }
    }
}
