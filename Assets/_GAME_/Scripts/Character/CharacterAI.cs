using UnityEngine;

public abstract class CharacterAI : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;
    public Transform player;
    public SpriteRenderer spriteRenderer;
    public LootBag lootBag;

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
    }

    public void Move(Vector2 direction)
    {
        rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);
        
        anim.SetFloat("xVelocity", direction.x);
        anim.SetFloat("yVelocity", direction.y);
        anim.SetFloat("speed", moveSpeed);    
    }

    public abstract void ChangeState(CharacterState newState);
    
    public void EnsurePlayer()
    {
        if (player != null) return;

        var obj = GameObject.FindGameObjectWithTag("Player");
        if (obj != null)
        {
            player = obj.transform;
            Debug.Log($"{name} found player.");
        }
    }
}