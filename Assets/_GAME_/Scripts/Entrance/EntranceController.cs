using UnityEngine;
using UnityEngine.UI;

public class EntranceController : MonoBehaviour
{
    [SerializeField] private GameEventChannel EntranceChannel;
    [SerializeField] private Text gameInfo;
    [SerializeField] private Image keyInfo;


    private void Awake()
    {
        //gameInfo.enabled = false;
        keyInfo.enabled = false;
    }

    private void OnEnable()
    {
        EntranceChannel.OnEventRaised += TogglePrompt;
    }

    private void OnDisable()
    {
        EntranceChannel.OnEventRaised -= TogglePrompt;
    }

    private void TogglePrompt(bool show)
    {
        //gameInfo.enabled = show;
        keyInfo.enabled= show;
    }
}
