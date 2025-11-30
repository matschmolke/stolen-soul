using UnityEngine;

public class Movements : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    private float walkSpeed = 3f;
    private float runSpeed = 6f;

    //remembers the direction
    private Vector2 lastDirection = Vector2.down;

    private bool canAttack = true;

    public bool isDead = false;

    public Transform attackOrigin;
    public float attackRadius = 1.5f;
    public LayerMask enemyMask;

    public int attackDamage = 15;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
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

            canAttack = false;
        }
    }

    //function called at the end of the attack animation
    public void EndAttack()
    {
        canAttack = true;
    }

    //for testing purposes
    public void Hurt()
    {
        //add slowing down player for a second
        anim.SetTrigger("isHurt");
    }

    public void Dead()
    {
        anim.SetTrigger("isDead");
        rb.linearVelocity = Vector2.zero;
        isDead = true;
        transform.GetChild(0).gameObject.SetActive(false); //Disable Trigger Collider
    }

    //deal damage to enemy
    public void DealDamage()
    {
        Collider2D[] targetsInRange = Physics2D.OverlapCircleAll(attackOrigin.position, attackRadius);

        foreach (Collider2D collider in targetsInRange)
        {
            IDamageable damageable = collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(attackDamage);
            }
        }
    }
}
