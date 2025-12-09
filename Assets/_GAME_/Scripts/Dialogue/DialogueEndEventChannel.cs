using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "DialogueEndEventChannel", menuName = "Events/DialogueEndEventChannel")]
public class DialogueEndEventChannel : ScriptableObject
{
    public UnityAction OnEventRaised;

    public void RaiseEvent()
    {
        OnEventRaised?.Invoke();
    }
}
