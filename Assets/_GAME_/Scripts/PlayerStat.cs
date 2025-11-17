using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;
    
    public int maxScore = 100;

    public Image healthImage; // Image with Type = Filled
    public Image manaImage; // Image with Type = Filled
    public Text healthText;
    public Text manaText;

    private int currentHealthScore;
    private int currentManaScore;
    
    public int CurrentHealthScore {get => currentHealthScore;}
    public int CurrentManaScore {get => currentManaScore;}
    
    private Movements playerScript;

    // Events for the future
    //public event Action<int, int> OnHealthChanged; // (current, max)
    //public event Action<int, int> OnManaChanged;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealthScore = maxScore;
        currentManaScore = maxScore;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Movements>();
    }

    void UpdateText()
    {
        healthText.text = $"{currentHealthScore}%";
        manaText.text = $"{currentManaScore}%";

        if (currentHealthScore == 0)
        {
            playerScript.Dead();
        }
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
        currentHealthScore += amount;
    }

    public void TakeDamage(int dmg)
    {
        currentHealthScore -= dmg;
    }

    public void UseMana(int spellCost)
    {
        currentManaScore -= spellCost;
    }

    public void RestoreMana(int amount)
    {
        if (amount <= 0) return;
        SetMana(currentManaScore + amount);
    }

    public void RefillAll()
    {
        currentHealthScore = maxScore;
        currentManaScore = maxScore;
    }

    public void KillPlayer()
    {
        currentHealthScore = 0;
        currentManaScore = 0;
    }

    private void UpdateUI()
    {
        UpdateText();

        float healthFillValue = (float)currentHealthScore / (float)maxScore;
        float manaFillValue = (float)currentManaScore / (float)maxScore;

        // defensywnie dopilnujemy zakresu 0..1
        healthFillValue = Mathf.Clamp01(healthFillValue);
        manaFillValue = Mathf.Clamp01(manaFillValue);

        // ustawiamy fill (Image.Type musi by� Filled)
        if (healthImage != null) healthImage.fillAmount = healthFillValue;
        if (manaImage != null) manaImage.fillAmount = manaFillValue;
    }

    // Update is called once per frame
    void Update()
    {
        // Temporary
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            TakeDamage(10);
        }
        else if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            RefillAll();
        }
        else if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            Heal(10);
        }
        else if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            UseMana(10);
        }
        else if (Keyboard.current.zKey.wasPressedThisFrame)
        {
            KillPlayer();
        }

        if (currentHealthScore < 0)
        {
            currentHealthScore = 0;

        }
        else if (currentManaScore < 0)
        {
            currentManaScore = 0;
        }

        if (currentHealthScore > maxScore)
        {
            currentHealthScore = maxScore;
        }
        else if (currentManaScore > maxScore)
        {
            currentManaScore = maxScore;
        }

        this.UpdateUI();
    }
}
