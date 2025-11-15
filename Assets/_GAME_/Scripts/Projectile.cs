using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector2 direction;
    public float speed = 10f;

    public void Shoot(Vector3 target)
    {
        direction = (target - transform.position).normalized;
    }

    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }
}