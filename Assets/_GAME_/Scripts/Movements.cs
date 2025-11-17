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

    private bool isDead = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isDead) return;
        MovePlayer();
        HandleAttack();
        Hurt();
        KillPlayer();
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
        
        if (Input.GetKeyDown(KeyCode.B) && canAttack)
        {
            anim.SetTrigger("isAttacking");
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
        if (Input.GetKeyDown(KeyCode.R))
        {
            //add slowing down player for a second
            anim.SetTrigger("isHurt");
        }
    }

    public void Dead()
    {
        anim.SetTrigger("isDead");
        isDead = true;
    }

    private void KillPlayer()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Dead();
        }
    }


}
