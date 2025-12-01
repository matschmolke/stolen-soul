using UnityEngine;

public class EnemyChaseState : CharacterState
{
    private EnemyAI enemy;
    private Breadcrumb targetBreadcrumb = null;

    public EnemyChaseState(EnemyAI enemy) : base(enemy)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        enemy.moveSpeed = enemy.Data.runSpeed;
        Debug.Log("Enemy run speed:" + enemy.moveSpeed);
    }

    public override void Update()
    {
        Vector2 targetPos;
        
        if (enemy.CanSeePlayer())
        {
            targetBreadcrumb = null;
            targetPos = enemy.GetPlayerPos();

            float distance = Vector2.Distance(enemy.GetMyPos(), targetPos);
            if (distance <= enemy.Data.attackRange)
            {
                enemy.ChangeState(enemy.attackState);
                return;
            }
        }
        else
        {
            var crumb = FindNextBreadcrumb();
            if (crumb != null)
            {
                targetPos = crumb.position;
            }
            else
            {
                enemy.ChangeState(enemy.idleState);
                return;
            }
        }

        Vector2 dir = (enemy.GetPlayerPos() - enemy.GetMyPos()).normalized;
        enemy.Move(dir, enemy.Data.walkSpeed);
    }
    
    private Breadcrumb FindNextBreadcrumb()
    {
        if (enemy.breadcrumbTrail == null) return null;

        var crumbs = enemy.breadcrumbTrail.GetBreadcrumbs();
        if (crumbs == null || crumbs.Count == 0) return null;

        Vector2 myPos = enemy.GetMyPos();

        if (targetBreadcrumb != null)
        {
            while (targetBreadcrumb.next != null)
            {
                Breadcrumb nextCrumb = targetBreadcrumb.next;
                float dist = Vector2.Distance(myPos, nextCrumb.position);

                if (dist <= enemy.Data.visionRange && !enemy.IsBlocked(nextCrumb.position))
                    targetBreadcrumb = nextCrumb;
                else
                    break;
            }

            return targetBreadcrumb;
        }

        Breadcrumb closest = null;
        float minDist = Mathf.Infinity;

        foreach (var crumb in crumbs)
        {
            if (enemy.IsBlocked(crumb.position)) continue;

            float dist = Vector2.Distance(myPos, crumb.position);
            if (dist > enemy.Data.visionRange) continue;

            if (dist < minDist)
            {
                minDist = dist;
                closest = crumb;
            }
        }

        targetBreadcrumb = closest;
        return targetBreadcrumb;
    }
}