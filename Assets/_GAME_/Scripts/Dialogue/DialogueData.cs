using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    [Header("Image for dialogue")]
    public Sprite image;

    [Header("Dialogue Lines")]
    [TextArea(1, 5)]
    public List<string> lines;
}
