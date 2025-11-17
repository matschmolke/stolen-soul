using UnityEngine;
using UnityEngine.UI;

public class EffectSlotUI : MonoBehaviour
{
    public Image iconImage;

    public void SetEffect(Effect effect)
    {
        Debug.Log("Effects Slot UI : void SetEffect");
        if (effect != null)
        {
            Debug.Log("Effects SLot UI: effect != null, CHANGING ICON");
            iconImage.sprite = effect.icon;
            iconImage.enabled = true;
        }
        else
        {
            Debug.Log("Effects SLot UI: effect IS NULL");
            iconImage.enabled = false;
        }
    }
}
