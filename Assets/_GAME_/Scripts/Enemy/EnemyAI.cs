using UnityEngine;

public class EnemyAI : CharacterAI
{
    public EnemyData Data;

    [Header("Layer Masks")]
    public LayerMask playerLayerMask;
    public LayerMask obsticleLayerMask;

    // States
    public CharacterState currentState;
    public EnemyIdleState idleState;
    public EnemyChaseState chaseState;
    public EnemyAttackState attackState;
    public EnemyDeadState deadState;

    [HideInInspector] public float currentHealth;
    [HideInInspector] public bool canAttack = true;
    [HideInInspector] public bool isDead = false;
    
    public static event System.Action<EnemyAI> OnEnemyAttack;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        idleState = new EnemyIdleState(this);
        chaseState = new EnemyChaseState(this);
        attackState = new EnemyAttackState(this);
        deadState = new EnemyDeadState(this);

        ChangeState(idleState);
    }

    public override void ChangeState(CharacterState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    void Update()
    {
        currentState?.Update();
    }

    // ----------- INIT -----------
    public void Init(EnemyData enemyData)
    {
        Data = enemyData;

        base.Init(enemyData);

        currentHealth = Data.maxHealth;
    }
    public void OnAttackEnd()
    {
        canAttack = true;
    }

    public void TriggerAttack()
    {
        OnEnemyAttack?.Invoke(this);
    }

    // ----------- VISION -----------
    public bool CanSeePlayer()
    {
        Vector2 pos = GetMyPos();
        Vector2 target = GetPlayerPos();
        float dist = Vector2.Distance(pos, target);

        if (dist > Data.visionRange) return false;

        Vector2 dir = target - pos;

        RaycastHit2D hit = Physics2D.Raycast(
            pos,
            dir,
            Data.visionRange,
            playerLayerMask | obsticleLayerMask
        );

        return hit.collider != null && hit.collider.CompareTag("Player");
    }

    public Vector2 GetMyPos() => transform.position;
    public Vector2 GetPlayerPos() => player.position;

    // ----------- DAMAGE -----------
    public void TakeDamage(float dmg)
    {
        Debug.LogWarning("Damage taken by enemy");
        if (isDead) return;

        currentHealth -= dmg;
        anim.SetTrigger("isHurt");

        if (currentHealth <= 0)
        {
            ChangeState(deadState);
        }
    }
    
    public void SetPlayer(Transform p)
    {
        player = p;
    }
}
