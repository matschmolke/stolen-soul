using UnityEngine;

public class NPCTalkState : CharacterState
{
    private NPCAI npc;

    public NPCTalkState(NPCAI npc) : base(npc)
    {
        this.npc = npc;
    }

    public override void Enter()
    {
        npc.anim.SetTrigger("isTalking");
        Debug.Log("NPC: Hello traveler!");
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            npc.ChangeState(npc.idleState);
        }
    }

    public override void Exit()
    {
        Debug.Log("NPC stopped talking.");
    }
}