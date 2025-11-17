using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New Scroll Item", menuName = "Items/New Scroll Item")]
public class Scroll : ItemBase
{
    public void Awake()
    {
        Id = GenerateId();
        itemType = ItemType.Scroll;
    }

    protected override void GenerateDescription()
    {
        description = "Grants player /spell name/ spell. How cool is that?";
    }
}
