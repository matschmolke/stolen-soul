using UnityEngine;

public class NPCTalkState : CharacterState
{
    private NPCAI npc;
    private bool isTradeOpen = false;

    public NPCTalkState(NPCAI npc) : base(npc)
    {
        this.npc = npc;
    }

    public override void Enter()
    {
        npc.anim.SetTrigger("isTalking");
        
        string message = npc.Data.isTrader ? "NPC: Hello traveler! Want to trade?" : "NPC: Hello traveler!";
        Debug.Log(message);
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
            // Відкрити трейд
            if (TradeManager.Instance != null)
            {
                TradeManager.Instance.TradeWindow.SetActive(true);
                Time.timeScale = 0;
                TradeManager.Instance.RefreshUI();
            }
            isTradeOpen = true;
        }
        else
        {
            // Закрити трейд
            if (TradeManager.Instance != null)
            {
                TradeManager.Instance.TradeWindow.SetActive(false);
                Time.timeScale = 1;
                TradeManager.Instance.DeselectAllSlots();
            }
            isTradeOpen = false;
            npc.ChangeState(npc.idleState);
        }
    }

    public override void Exit()
    {
        Debug.Log("NPC stopped talking.");
    }
}