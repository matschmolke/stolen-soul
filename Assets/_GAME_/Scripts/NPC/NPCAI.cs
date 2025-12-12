using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCAI : CharacterAI, IDamageable
{
        public NPCData Data;

        public float interactionDistance = 2f;
        public KeyCode interactionKey = KeyCode.Space;

        public GameObject enemyAIPrefab;
        
        public CharacterState currentState;
        public NPCIdleState idleState;
        public NPCAggressiveState aggressiveState;
        public NPCTalkState talkState;

        public bool isAggressive = false;

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
            aggressiveState = new NPCAggressiveState(this);
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

        public void TakeDamage(float damage)
        {
                if (Data.isTrader) return;

                Debug.Log($"NPC took {damage} damage");
                if (!isAggressive)
                {
                        isAggressive = true;
                        ChangeState(aggressiveState);

                        BecomeEnemy();
                        return;
                }
        }
        
        public void BecomeEnemy()
        {
                Debug.Log("NPC became hostile!");

                var npcData = this.Data;
                var player = this.player;

                this.enabled = false;

                EnemyAI enemy = GetComponent<EnemyAI>();
                enemy.enabled = true;

                enemy.Data = npcData.enemyVersion;
                enemy.Init(enemy.Data);
                enemy.SetPlayer(player);

                Debug.Log("NPC successfully converted to EnemyAI!");
        }

        CharacterData IDamageable.Data => Data;
}