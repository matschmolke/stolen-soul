using System.Collections;
using UnityEngine;
using System;
using System.Diagnostics;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class Movements : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    private float walkSpeed = 3f;
    private float runSpeed = 6f;

    //remembers the direction
    private Vector2 lastDirection = Vector2.down;

    public bool canAttack = true;

    public bool isDead = false;

    public Transform attackOrigin;
    public float attackRadius = 1.5f;
    public LayerMask enemyMask;

    public int attackDamage = 15;

    private bool deadScreen = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
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

        if(deadScreen) return;
        deadScreen = true;
        StartCoroutine(WaitforDiedScene());
    }

    private IEnumerator WaitforDiedScene()
    {
        yield return new WaitForSeconds(5f);

        UnityEngine.SceneManagement.SceneManager.LoadScene("PlayerDied");
    }

    //deal damage to enemy
    public void DealDamage()
    {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(attackOrigin.position, attackRadius, enemyMask);

        foreach (Collider2D enemyCollider in enemiesInRange)
        {
            IDamageable character;
            
            try
            {
                character = enemyCollider.GetComponent<IDamageable>();
                if (character != null)
                {
                    character.TakeDamage(attackDamage);
                }
                else
                {
                    Debug.Log("Character not found");
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }
}
