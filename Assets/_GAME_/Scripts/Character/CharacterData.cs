using UnityEngine;

public class CharacterData : ScriptableObject
{
    [Header("General")] 
    public string characterName;
    public RuntimeAnimatorController animatorController;
    public Sprite sprite;

    [Header("Stats")]
    public float maxHealth = 100f;
    public float walkSpeed = 3f;
    public float runSpeed = 5f;

    [Header("Sounds")]
    public SoundType soundHurt;
    public SoundType soundFootSteps;
    public SoundType soundAttach;
    public SoundType souncDeath;
}