using System.Collections;
using UnityEngine;
using static EnemySaveData;

public class EnemyAI : CharacterAI
{
    //for save/load
    public string spawnId;

    public EnemyData Data;

    // States
    public CharacterState currentState;
    public EnemyIdleState idleState;
    public EnemyChaseState chaseState;
    public EnemyAttackState attackState;
    public EnemyDeadState deadState;

    [HideInInspector] public float currentHealth;
    [HideInInspector] public bool canAttack = true;
    [HideInInspector] public bool isDead = false;
    [HideInInspector] public bool bossMusicStarted = false;

    public static event System.Action<EnemyAI> OnEnemyAttack;

    // Breadcrumbs for chasing
    private ContextSteering contextSteering;
    public BreadcrumbTrail breadcrumbTrail;
    public Breadcrumb targetBreadcrumb = null;
    
    protected override void Awake()
    {
        base.Awake();

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        lootBag = GetComponent<LootBag>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (Data)
        {
            Init(Data);
        }

        contextSteering = new ContextSteering(this.gameObject, obsticleLayerMask | enemyLayerMask);
    }

    void Start()
    {
        idleState = new EnemyIdleState(this);
        chaseState = new EnemyChaseState(this);
        attackState = new EnemyAttackState(this);
        deadState = new EnemyDeadState(this);

        ChangeState(idleState);

        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                breadcrumbTrail = playerObj.GetComponent<BreadcrumbTrail>();
            }
        }
    }

    void Update()
    {
        currentState?.Update();
    }

    public override void ChangeState(CharacterState newState)
    {
        if (isDead && newState != deadState) return;

        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }


    public void Init(EnemyData enemyData)
    {
        Data = enemyData;
        base.Init(enemyData);
        currentHealth = Data.maxHealth;
        anim.runtimeAnimatorController = Data.animatorController;

        if (lootBag != null)
            lootBag.SetLoot(Data.lootTable);
        else
            Debug.LogWarning($"{name} has no LootBag!");
    }

    public void OnAttackEnd()
    {
        canAttack = true;
    }

    public void TriggerAttack()
    {
        OnEnemyAttack?.Invoke(this);
    }

    // ----------- VISION & MOVEMENT -----------
    public bool CanSeePlayer()
    {
        if (player == null) return false;

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

        return hit.collider != null && hit.collider.CompareTag("PlayerTriggerCollider");
    }

    public bool IsBlocked(Vector2 targetPos)
    {
        Vector2 origin = GetMyPos();
        Vector2 dir = targetPos - origin;
        float dist = dir.magnitude;

        return Physics2D.Raycast(origin, dir.normalized, dist, obsticleLayerMask);
    }
    
    // ----------- DAMAGE & DEATH -----------
    public void TakeDamage(float dmg)
    {
        if (isDead) return;

        currentHealth -= dmg;
        SoundManager.PlaySound(Data.soundHurt);
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
