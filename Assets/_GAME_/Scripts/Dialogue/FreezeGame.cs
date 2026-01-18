using UnityEngine;

public static class FreezeGame
{
    public static bool DialogueActive { get; private set; }

    public static void LockForDialogue()
    {
        DialogueActive = true;
    }

    public static void UnlockAfterDialogue()
    {
        DialogueActive = false;
    }
}
