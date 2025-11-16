using UnityEngine;

public abstract class StaticEffect : ScriptableObject
{
    [TextArea]
    public string description;

    public abstract void Apply(PlayerStats1 stats);
    public abstract void Remove(PlayerStats1 stats);

    protected void OnValidate()
    {
        GenerateDescription();
    }

    protected abstract void GenerateDescription();
}
