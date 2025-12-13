using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCAI : CharacterAI
{
        public NPCData Data;

        public float interactionDistance = 2f;
        public KeyCode interactionKey = KeyCode.Space;
        public CharacterState currentState;
        public NPCIdleState idleState;
        public NPCTalkState talkState;

        private TraderInventory traderInventory;
        
        protected override void Awake()
        {
                base.Awake();
        }

        void Start()
        {
            if (Data.isTrader)
            {
                
                traderInventory = transform.AddComponent<TraderInventory>();
                traderInventory.Initalize(Data.DefaultTraderInventory);

                if (traderInventory == null)
                {
                    Debug.LogError("Trader NPC is missing TraderInventory component!");
                }
            }

            idleState = new NPCIdleState(this);
            talkState = new NPCTalkState(this, traderInventory);

            ChangeState(idleState);
        }

        void Update()
        {
                EnsurePlayer();
                if (player == null) return;

                currentState?.Update();
        }

        public override void ChangeState(CharacterState newState)
        {
                currentState?.Exit();
                currentState = newState;
                currentState.Enter();
        }
}