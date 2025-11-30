using UnityEngine;

public class NPCIdleState : CharacterState
{
    private NPCAI npc;
    
    public NPCIdleState(NPCAI npc) : base(npc)
    {
        this.npc = npc;
    }

    public override void Enter()
    {
        npc.anim.SetFloat("speed", 0);
    }

    public override void Update()
    {
        if (npc.player == null)
        {
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                npc.player = playerObj.transform;
            else
                return;
        }

        float dist = Vector2.Distance(npc.transform.position, npc.player.position);

        if (dist < npc.Data.talkRange)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                npc.ChangeState(npc.talkState);
            }
        }
    }
    
    public override void Exit()
    {
        
    }
}