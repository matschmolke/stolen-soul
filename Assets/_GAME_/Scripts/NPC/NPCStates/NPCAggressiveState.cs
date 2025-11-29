using UnityEngine;

public class NPCPatrolState : CharacterState
{
    private NPCAI npc;

    public NPCPatrolState(NPCAI npc) : base(npc)
    {
        this.npc = npc;
    }
}