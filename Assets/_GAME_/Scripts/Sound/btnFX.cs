using UnityEngine;

public class btnFX : MonoBehaviour
{
    public void HoverSound()
    {
        SoundManager.PlaySound(SoundType.HOVER_MENU);
    }

    public void ClickSound()
    {
        SoundManager.PlaySound(SoundType.CLICK_MENU);
    }
}
