using System.Collections.Generic;
using UnityEngine;

public class ContextSteering
{
    public float obstacleDetectionDistance = 3f;
    public LayerMask obstacleLayerMask;
    public GameObject actor;

    // Arrays storing weights for Interest and Danger
    private readonly float[] Interest = new float[8];
    private readonly float[] Danger = new float[8];

    // For debug purposes
    private readonly float[] Weights = new float[8];

    // Predefined 8-direction unit vectors (N, NE, E, SE, S, SW, W, NW)
    private readonly Vector2[] directions =
    {
        new Vector2(0,1).normalized,
        new Vector2(1,1).normalized,
        new Vector2(1,0).normalized,
        new Vector2(1,-1).normalized,
        new Vector2(0,-1).normalized,
        new Vector2(-1,-1).normalized,
        new Vector2(-1,0).normalized,
        new Vector2(-1,1).normalized
    };

    public ContextSteering(GameObject actor, LayerMask obsticleLayerMask)
    {
        this.actor = actor;
        this.obstacleLayerMask = obsticleLayerMask;
    }

    public Vector2 GetSteeringDirection(Vector2 origin, Vector2 toTargetDir)
    {
        CalculateInterestWeigths(toTargetDir);
        CalculateDangerWeigths(origin);

        Vector2 finalDir = CalculateDirectionVector();

        for(int i = 0; i < directions.Length; i++)
        {
            Debug.DrawRay(origin, directions[i] * Weights[i], Color.darkViolet);
        }

        Debug.DrawRay(origin, finalDir, Color.blue);

        ResetWeights();

        return finalDir.normalized;
    }

    private Vector2 CalculateDirectionVector()
    {
        Vector2 sum = Vector2.zero;
        float totalWeight = 0f;

        for (int i = 0; i < directions.Length; i++)
        {
            float weight = Mathf.Clamp01(Interest[i] - Danger[i]);

            Weights[i] = weight;

            if (weight == 0) continue;

            sum += directions[i] * weight;
            totalWeight += weight;
        }

        if (totalWeight == 0) return Vector2.zero;

        return sum / totalWeight;
    }

    private void CalculateInterestWeigths(Vector2 targetDirection)
    {
        for (int i = 0; i < directions.Length; i++)
        {
            // Returns value > 0 when the direction points generally toward the target (angle < 90°)
            float weight = Vector2.Dot(targetDirection, directions[i]);

            Interest[i] = Mathf.Clamp01(weight);
        }
    }

    private void CalculateDangerWeigths(Vector2 origin)
    {
        Collider2D[] obstacles = DetectObstacles(origin);

        float minSafeDistance = .6f;

        foreach (Collider2D obstacleCollider in obstacles)
        {
            if(obstacleCollider.gameObject == actor) continue;

            Vector2 closestPoint = obstacleCollider.ClosestPoint(origin);

            Vector2 directionToObstacle = closestPoint - origin;

            float distanceToObstacle = directionToObstacle.magnitude;

            //calculate weight based on the distance Enemy<--->Obstacle
            float weight
                = distanceToObstacle <= minSafeDistance
                ? 1
                : (obstacleDetectionDistance - distanceToObstacle) / obstacleDetectionDistance;

            Vector2 directionToObstacleNormalized = directionToObstacle.normalized;

            //Add obstacle parameters to the danger array
            for (int i = 0; i < directions.Length; i++)
            {
                float result = Vector2.Dot(directionToObstacleNormalized, directions[i]);

                float valueToPutIn = result * weight;

                //override value only if it is higher than the current one stored in the danger array
                if (valueToPutIn > Danger[i])
                {
                    Danger[i] = valueToPutIn;
                }
            }
        }
    }

    private void ResetWeights()
    {         
        for (int i = 0; i < directions.Length; i++)
        {
            Interest[i] = 0f;
            Danger[i] = 0f;
            Weights[i] = 0f;
        }
    }

    private Collider2D[] DetectObstacles(Vector2 origin)
    {
        return Physics2D.OverlapCircleAll(origin, obstacleDetectionDistance, obstacleLayerMask);
    }
}
