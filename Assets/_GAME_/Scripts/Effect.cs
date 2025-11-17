using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effect")]
public class Effect : ScriptableObject
{
    public string effectName;
    public Sprite icon;
    public float duration;
}
