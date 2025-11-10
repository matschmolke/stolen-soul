using UnityEngine;
using UnityEngine.UI;

public class DoorController : MonoBehaviour
{
    [SerializeField] private GameEventChannel DoorChannel;
    [SerializeField] private Text gameInfo;
    [SerializeField] private Image keyInfo;

    private void Awake()
    {
        gameInfo.enabled = false;
        keyInfo.enabled = false;
    }

    private void OnEnable()
    {
        DoorChannel.OnEventRaisedKey += TogglePrompt;
    }

    private void OnDisable()
    {
        DoorChannel.OnEventRaisedKey -= TogglePrompt;
    }

    private void TogglePrompt(bool show, bool key)
    {
        if(show && key)
        {
            gameInfo.enabled = show;
            gameInfo.text = "[ The key is required ]";
        }
        else if(show && !key)
        {
            keyInfo.enabled = show;
        }
        else
        {
            gameInfo.enabled = show;
            keyInfo.enabled = show;
        }
    }
}
