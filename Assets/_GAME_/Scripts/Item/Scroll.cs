using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New Scroll Item", menuName = "Items/New Scroll Item")]
public class Scroll : ItemBase
{
    public string spellName;
    public Spell spellData;
    public bool isOneTimeUse = false;
    public void Awake()
    {
        Id = GenerateId();
        itemType = ItemType.Scroll;
    }

    protected override void GenerateDescription()
    {
        description = isOneTimeUse == false ? $"Grants player {spellName} spell. How cool is that?"
            : $"Grants player {spellName} effect. One time use only!";
    }
}
