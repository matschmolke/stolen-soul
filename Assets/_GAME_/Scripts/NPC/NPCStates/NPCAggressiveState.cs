using UnityEngine;

public class NPCAggressiveState : CharacterState
{
    private NPCAI npc;

    public NPCAggressiveState(NPCAI npc) : base(npc)
    {
        this.npc = npc;
    }

    public override void Enter()
    {
        Debug.Log("NPC entered AGGRESSIVE state.");
        npc.anim.SetTrigger("isAggressive");
    }

    public override void Update()
    {
        if (npc.player == null) return;

        float dist = Vector2.Distance(npc.transform.position, npc.player.position);
        Vector2 dir = (npc.player.position - npc.transform.position).normalized;
        
        if (dist > npc.Data.hostileAttackRange * 1.2f)
        {
            npc.Move(dir, npc.moveSpeed);
        }
    }
}