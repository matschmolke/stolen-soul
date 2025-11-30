using UnityEngine;
using UnityEngine.UI;

public class EffectSlotUI : MonoBehaviour
{
    public Image iconImage;

    public void SetEffect(Effect effect)
    {
        if (effect != null)
        {
            iconImage.sprite = effect.icon;
            iconImage.enabled = true;
        }
        else
        {
            iconImage.enabled = false;
        }
    }
}
