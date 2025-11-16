using TMPro;
using UnityEngine;

public class StatUI : MonoBehaviour
{
    [SerializeField] private TMP_Text statText;
    [SerializeField] private string statName;

    private PlayerStats1 playerStats;

    private void Start()
    {
        statText = GetComponent<TMP_Text>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerStats1>();

        switch (statName)
        {
            case "AT":
                playerStats.OnAttackDamageChanged += UpdateValue;
                UpdateValue(playerStats.Attack);
                break;
            case "DF":
                playerStats.OnDefenceChanged += UpdateValue;
                UpdateValue(playerStats.Defence);
                break;
        }
    }

    public void UpdateValue(int value)
    {
        statText.text = $"{statName}\n{value}";
    }
}
