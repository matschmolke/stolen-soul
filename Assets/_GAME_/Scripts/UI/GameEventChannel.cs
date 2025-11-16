using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GameEventChannel", menuName = "Events/GameEventChannel")]
public class GameEventChannel : ScriptableObject
{
    public UnityAction<bool> OnEventRaised;
    public UnityAction<bool,bool> OnEventRaisedKey;

    public void RaiseEvent(bool value)
    {
        OnEventRaised?.Invoke(value);
    }

    public void RaiseEvent(bool value, bool key)
    {
        OnEventRaisedKey?.Invoke(value,key);
    }
}
