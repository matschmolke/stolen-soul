using UnityEngine;

public class AttackRaycast : MonoBehaviour
{
    private GameObject player;

    [SerializeField]
    private LayerMask obstaclesLayerMask;

    public EnemyAI enemyAI;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        Vector2 origin = transform.position;
        //offset 
        Vector2 targetPos = (Vector2)player.transform.position + Vector2.down * 0.70f;
        Vector2 dir = targetPos - (Vector2)transform.position;
        float dist = dir.magnitude;

        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, dir.normalized, dist, obstaclesLayerMask);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider == null) continue;

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("PlayerBody"))
            {
                //can attack player
                enemyAI.isObstacleInTheWay = false;
                break;
            }
            else
            {
                //obstacle in the way
                enemyAI.isObstacleInTheWay = true;
                break;
            }
        }

        Debug.DrawRay(origin, dir, enemyAI.isObstacleInTheWay ? Color.red : Color.green);
    }
}
