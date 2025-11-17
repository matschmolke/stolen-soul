using UnityEngine;

public abstract class StaticEffect : ScriptableObject
{
    [TextArea]
    public string description;

    public abstract void Apply(PlayerStats stats);
    public abstract void Remove(PlayerStats stats);

    protected void OnValidate()
    {
        GenerateDescription();
    }

    protected abstract void GenerateDescription();
}
