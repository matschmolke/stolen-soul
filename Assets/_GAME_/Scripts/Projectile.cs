using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector2 direction;
    public float speed;

    public void Shoot(Vector3 target, float spellSpeed)
    {
        direction = (target - transform.position).normalized;
        speed = spellSpeed;
    }

    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
        Debug.Log("Position of projectile: " + transform.position);
    }
}