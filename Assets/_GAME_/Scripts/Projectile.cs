using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector2 direction;
    public float speed;
    private Animator animator;
    
    void Awake()
    {
      animator = GetComponent<Animator>();  
    }
    public void Shoot(Vector3 target, float spellSpeed)
    {
        direction = (target - transform.position).normalized;
        speed = spellSpeed;

        if (animator != null)
        {
            animator.SetTrigger(("CastTrigger"));
        }
    }

    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }
}