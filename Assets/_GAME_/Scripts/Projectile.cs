using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector2 direction;
    public float speed;
    private Animator animator;

    private float damage;

    void Awake()
    {
      animator = GetComponent<Animator>();  
    }
    public void Shoot(Vector3 target, float spellSpeed, float dmg)
    {
        direction = (target - transform.position).normalized;
        speed = spellSpeed;
        damage = dmg;

        if (animator != null)
        {
            animator.SetTrigger(("CastTrigger"));
        }
    }

    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyAI enemy = collision.GetComponent<EnemyAI>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Map"))
        {
            Destroy(gameObject);
            return;
        }
    }

}