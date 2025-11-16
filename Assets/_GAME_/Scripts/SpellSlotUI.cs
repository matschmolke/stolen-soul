using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellSlotUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI keyText;
    public Image cooldownOverlay;

    public void SetSpell(Spell spell, int keyNumber)
    {
        icon.sprite = spell.icon;
        keyText.text = keyNumber.ToString();
    }

    public void SetCooldown(float fillAmount)
    {
        if (cooldownOverlay != null)
        {
            Debug.LogWarning("cooldown overlay is set to: " + fillAmount);
            cooldownOverlay.fillAmount = fillAmount;
        }
    }
}
