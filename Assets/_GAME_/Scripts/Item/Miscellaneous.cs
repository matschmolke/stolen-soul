using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New Miscellaneous Item", menuName = "Items/New Miscellaneous Item")]
public class Miscellaneous : ItemBase
{
    public void Awake()
    {
        Id = GenerateId();
        itemType = ItemType.Misc;
    }

    protected override void GenerateDescription()
    {
        description = "Shiny bit or not so shiny";
    }
}
