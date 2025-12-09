using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "DialogueEventChannel", menuName = "Events/DialogueEventChannel")]
public class DialogueEventChannel : ScriptableObject
{
    public UnityAction<DialogueData> OnEventRaised;

    public void RaiseEvent(DialogueData data)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(data);
    }
}
