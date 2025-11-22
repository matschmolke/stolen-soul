using System.Collections;
using UnityEngine;

public class Movements : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer playerRenderer;

    private PlayerStats playerStats;

    private float walkSpeed = 3f;
    private float runSpeed = 6f;

    //remembers the direction
    private Vector2 lastDirection = Vector2.down;

    public bool canAttack = true;

    public bool isDead = false;

    public Transform attackOrigin;
    public float attackRadius = 1.5f;
    public LayerMask enemyMask;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerRenderer = GetComponent<SpriteRenderer>();

        playerStats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        if (isDead)
        {
            rb.linearVelocity = Vector2.zero; 
            return;
        }

        MovePlayer();
        HandleAttack();
    }

    private void MovePlayer()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        Vector2 inputVector = new Vector2(xInput, yInput).normalized;
        Vector2 velocity = inputVector * currentSpeed;
        rb.linearVelocity = velocity;

        //last direction
        if (velocity.sqrMagnitude > 0.01f)
        {
            lastDirection = inputVector;
        }

        anim.SetFloat("xVelocity", velocity.x != 0 || velocity.y != 0 ? velocity.x : lastDirection.x);
        anim.SetFloat("yVelocity", velocity.x != 0 || velocity.y != 0 ? velocity.y : lastDirection.y);
        anim.SetFloat("speed", velocity.magnitude);
    }

    private void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            anim.SetTrigger("isAttacking");
            DealDamage();
        }
    }

    public void Hurt()
    {
        StartCoroutine(HurtAnim(playerRenderer));
    }

    private IEnumerator HurtAnim(SpriteRenderer sprite)
    {
        sprite.color = new Color(1f, 0.523f, 0.612f, 1f);

        yield return new WaitForSeconds(0.3f);

        sprite.color = Color.white;

    }

    public void Dead()
    {
        anim.SetTrigger("isDead");
        rb.linearVelocity = Vector2.zero;
        isDead = true;
    }

    //deal damage to enemy
    public void DealDamage()
    {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(attackOrigin.position, attackRadius, enemyMask);

        foreach (Collider2D enemyCollider in enemiesInRange)
        {
            EnemyAI enemy = enemyCollider.GetComponent<EnemyAI>();
            if (enemy != null)
                enemy.TakeDamage(playerStats.Attack);
        }
    }
}
