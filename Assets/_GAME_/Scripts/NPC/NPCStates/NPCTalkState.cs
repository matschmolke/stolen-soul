using UnityEngine;

public class NPCTalkState : CharacterState
{
    private NPCAI npc;
    private bool isTradeOpen = false;
    private TraderInventory inventory;

    public NPCTalkState(NPCAI npc, TraderInventory inventory) : base(npc)
    {
        this.npc = npc;
        this.inventory = inventory;
    }

    public override void Enter()
    {
        npc.anim.SetTrigger("isTalking");
    }

    public override void Update()
    {
        if (Vector2.Distance(npc.transform.position, npc.player.position) > npc.interactionDistance)
        {
            npc.ChangeState(npc.idleState);
            return;
        }

        if (npc.Data.isTrader && Input.GetKeyDown(KeyCode.Space))
        {
            ToggleTradeWindow();
        }
    }

    private void ToggleTradeWindow()
    {
        if (!isTradeOpen)
        {
            if (TradeManager.Instance != null)
            {
                TradeManager.Instance.OpenWindow(inventory);
            }
            
            npc.player.GetComponent<Movements>().canAttack = false;
            
            isTradeOpen = true;
        }
        else
        {
            if (TradeManager.Instance != null)
            {
                TradeManager.Instance.CloseWindow();
            }
            
            npc.player.GetComponent<Movements>().canAttack = true;
            
            isTradeOpen = false;
            npc.ChangeState(npc.idleState);
        }
    }

    public override void Exit()
    {
        Debug.Log("NPC stopped talking.");
    }
}