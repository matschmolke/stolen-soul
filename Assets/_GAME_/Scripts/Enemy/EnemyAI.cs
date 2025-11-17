using UnityEngine;
using System.Collections;
public enum State
{
    Idle,
    Chasing,
    Attacking,
    Dead
}

public class EnemyAI : MonoBehaviour
{
    [Header("Enemy Data")]
    public EnemyData Data;

    [Header("Layer Masks")]
    public LayerMask obsticleLayerMask;
    public LayerMask playerLayerMask;
    public LayerMask enemyLayerMask;

    [Header("Runtime Info")]
    public State currentState = State.Idle;

    // Movement and steering
    private ContextSteering contextSteering;
    private BreadcrumbTrail breadcrumbTrail;
    private Breadcrumb targetBreadcrumb = null;

    // Combat and Data
    private float currentHealth;
    private float attackdmg;
    private bool canAttack = true;
    private bool hasDied = false;

    // Death Animation
    public float deathBlinkDuration = 0.8f;
    public float deathBlinkFrequency = 0.1f;

    // Components
    private Rigidbody2D rb;
    private Animator anim;
    private LootBag lootBag;
    private SpriteRenderer spriteRenderer;

    // Target (We don't like him)
    private Transform player;

    public void Init(EnemyData data)
    {
        Data = data;
        currentHealth = Data.maxHealth;
        attackdmg = Data.attackDamage;

        anim.runtimeAnimatorController = Data.animatorController;
        lootBag.SetLoot(Data.lootTable);
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        lootBag = GetComponent<LootBag>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        contextSteering = new ContextSteering(this.gameObject, obsticleLayerMask | enemyLayerMask);
    }

    void Start()
    {
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

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                HandleIdle();
                break;
            case State.Chasing:
                HandleChasing();
                break;
            case State.Attacking:
                HandleAttacking();
                break;
            case State.Dead:
                HandleDeath();
                break;
        }
    }

    private void HandleIdle()
    {
        anim.SetFloat("speed", 0);
        if (CanSeePlayer())
        {
            currentState = State.Chasing;
        }
    }

    private void HandleChasing()
    {
        bool playerVisible = CanSeePlayer();

        Vector2 targetPos;

        if (playerVisible)
        {
            targetBreadcrumb = null;
            targetPos = GetPlayerPos();

            float distanceToTarget = Vector2.Distance(GetMyPos(), targetPos);

            if (!playerVisible && distanceToTarget < 0.1f)
            {
                currentState = State.Idle;
                return;
            }

            if (playerVisible && distanceToTarget <= Data.attackRange)
            {
                currentState = State.Attacking;
                return;
            }
        }
        else
        {
            var crumb = FindNextBreadcrumb();

            if (crumb != null)
            {
                float dist = Vector2.Distance(GetMyPos(), crumb.position);

                // Jeœli dotar³ do ostatniego znanego punktu
                if (dist < 0.2f)
                {
                    currentState = State.Idle;
                    targetBreadcrumb = null;
                    return;
                }

                targetPos = crumb.position;
            }
            else
            {
                currentState = State.Idle;
                return;
            }

        }

        Vector2 toTargetDir = (targetPos - GetMyPos()).normalized;

        move(toTargetDir, Data.runSpeed);
    }

    private void HandleAttacking()
    {
        if (!canAttack) return;

        if (Vector2.Distance(GetMyPos(), GetPlayerPos()) > Data.attackRange)
        {
            currentState = State.Chasing;
            return;
        }

        Vector2 attackDir = (GetPlayerPos() - GetMyPos()).normalized;

        anim.SetFloat("xVelocity", attackDir.x);
        anim.SetFloat("yVelocity", attackDir.y);
        anim.SetTrigger("isAttacking");
        // Attack logic here

        canAttack = false;
    }

    private void HandleDeath()
    {
        if (hasDied) return;
        anim.SetTrigger("isDead");
        hasDied = true;
        StartCoroutine(nameof(DeathCoroutine));
    }

    public IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(3);

        float timer = 0f;
        bool visible = true;

        while (timer < deathBlinkDuration)
        {
            visible = !visible;
            spriteRenderer.enabled = visible;

            timer += deathBlinkFrequency;
            yield return new WaitForSeconds(deathBlinkFrequency);
        }

        spriteRenderer.enabled = false;

        lootBag.DropLoot(transform.position);

        Destroy(gameObject);   
    }

    public void TakeDamage(float amount)
    {
        anim.SetTrigger("isHurt");

        currentHealth -= amount;

        if(currentHealth <= 0)
        {
            currentState = State.Dead;
        }
    }

    public void OnAttackEnd()
    {
        canAttack = true;
    }

    private Breadcrumb FindNextBreadcrumb()
    {
        if (breadcrumbTrail == null)
            return null;

        var crumbs = breadcrumbTrail.GetBreadcrumbs();
        if (crumbs == null || crumbs.Count == 0)
            return null;

        Vector2 myPos = GetMyPos();

        if (targetBreadcrumb != null)
        {
            while(targetBreadcrumb.next != null)
            {
                Breadcrumb nextCrumb = targetBreadcrumb.next;

                float distToTarget = Vector2.Distance(myPos, nextCrumb.position);

                if(distToTarget <= Data.visionRange && !IsBlocked(nextCrumb.position))
                {
                    targetBreadcrumb = nextCrumb;
                }
                else
                {
                    break;
                }
            }

            return targetBreadcrumb;
        }

        Breadcrumb bestCrumb = null;
        float closestDist = Mathf.Infinity;

        foreach (var crumb in crumbs)
        {
            if (IsBlocked(crumb.position)) continue;

            float dist = Vector2.Distance(myPos, crumb.position);
            if (dist > Data.visionRange) continue;

            if (dist < closestDist)
            {
                closestDist = dist;
                bestCrumb = crumb;
            }
        }

        targetBreadcrumb = bestCrumb;
        return targetBreadcrumb;
    }

    private bool IsBlocked(Vector2 targetPos)
    {
        Vector2 origin = GetMyPos();
        Vector2 dir = targetPos - origin;
        float dist = dir.magnitude;

        return Physics2D.Raycast(origin, dir.normalized, dist, obsticleLayerMask);
    }

    private void move(Vector2 direction, float moveSpeed)
    {
        Vector2 desiredDirection = contextSteering.GetSteeringDirection(GetMyPos(), direction);

        rb.MovePosition(rb.position + desiredDirection * moveSpeed * Time.deltaTime);

        anim.SetFloat("xVelocity", desiredDirection.x);
        anim.SetFloat("yVelocity", desiredDirection.y);
        anim.SetFloat("speed", moveSpeed);
    }

    private bool CanSeePlayer()
    {
        float distanceToPlayer = Vector2.Distance(GetMyPos(), GetPlayerPos());

        if (distanceToPlayer <= Data.visionRange)
        {
            Vector2 direction = (GetPlayerPos() - GetMyPos());

            RaycastHit2D hit = Physics2D.Raycast(GetMyPos(), direction, Data.visionRange, playerLayerMask | obsticleLayerMask);

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                Debug.DrawRay(GetMyPos(), direction, Color.green);

                return true;
            }
            else
            {
                Debug.DrawRay(GetMyPos(), direction, Color.red);
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    private Vector2 GetMyPos()
    {
        return transform.position;
    }

    private Vector2 GetPlayerPos()
    {
        return player.position;
    }
}