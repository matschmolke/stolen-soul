using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{
    public Image healthImage; // Image with Type = Filled
    public Image manaImage; // Image with Type = Filled
    public Text healthText;
    public Text manaText;

    private PlayerStats playerStats;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerStats>();

        UpdateHealthUI(playerStats.currentHealth, playerStats.MaxHealth);
        UpdateManaUI(playerStats.currentMana, playerStats.MaxMana);

        playerStats.OnHealthChanged += UpdateHealthUI;
        playerStats.OnManaChanged += UpdateManaUI;
    }

    private void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        healthText.text = $"{currentHealth}/{maxHealth}";
        healthImage.fillAmount = Mathf.Clamp01((float)currentHealth / maxHealth);
    }

    private void UpdateManaUI(int currentMana, int maxMana)
    {
        manaText.text = $"{currentMana}/{maxMana}";
        manaImage.fillAmount = Mathf.Clamp01((float)currentMana / maxMana);
    }
    
    private void OnDestroy()
    {
        if (playerStats == null) return;

        playerStats.OnHealthChanged -= UpdateHealthUI;
        playerStats.OnManaChanged -= UpdateManaUI;
    }
}
