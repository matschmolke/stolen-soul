using UnityEngine;

public abstract class CharacterAI : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;
    public Transform player;
    public SpriteRenderer spriteRenderer;
    public LootBag lootBag;
    
    [Header("Layer Masks")]
    public LayerMask playerLayerMask;
    public LayerMask obsticleLayerMask;
    public LayerMask enemyLayerMask;
    
    private ContextSteering contextSteering;
    
    public float moveSpeed = 2f;

    public virtual void Init(CharacterData data)
    {
        moveSpeed = data.walkSpeed;

        // base stat setup
        anim.runtimeAnimatorController = data.animatorController;
        spriteRenderer.sprite = data.sprite;

        if (lootBag != null)
            lootBag.SetLoot(data.lootTable);
        else
            Debug.LogWarning($"{name} has no LootBag!");
    }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lootBag = GetComponent<LootBag>();
        
        contextSteering = new ContextSteering(this.gameObject, obsticleLayerMask | enemyLayerMask);
    }
    
    public void Move(Vector2 direction, float moveSpeed)
    {
        Vector2 desiredDirection = contextSteering.GetSteeringDirection(GetMyPos(), direction);
        rb.MovePosition(rb.position + desiredDirection * moveSpeed * Time.deltaTime);

        anim.SetFloat("xVelocity", desiredDirection.x);
        anim.SetFloat("yVelocity", desiredDirection.y);
        anim.SetFloat("speed", moveSpeed);
    }

    public Vector2 GetMyPos() => transform.position;
    public Vector2 GetPlayerPos() => player.position;

    public abstract void ChangeState(CharacterState newState);
    
    public void EnsurePlayer()
    {
        if (player != null) return;

        var obj = GameObject.FindGameObjectWithTag("Player");
        if (obj != null)
        {
            player = obj.transform;
        }
    }
}