using UnityEngine;

public class EnemyChaseState : CharacterState
{
    private EnemyAI enemy;
    private Breadcrumb targetBreadcrumb = null;
    private float footstepTimer = 0f;
    private float footstepInterval = 0.5f;

    public EnemyChaseState(EnemyAI enemy) : base(enemy)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        enemy.moveSpeed = enemy.Data.runSpeed;

        if (enemy.Data.bossType != BossType.None && !enemy.bossMusicStarted)
        {
            enemy.bossMusicStarted = true;

            switch (enemy.Data.bossType)
            {
                case BossType.Basic:
                    SoundManager.PlayBossMusic(BossType.Basic);
                    break;

                case BossType.Final:
                    SoundManager.PlayBossMusic(BossType.Final);
                    break;
            }
        }
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
                Debug.Log("Lost the player, switching to idle.");
                enemy.ChangeState(enemy.idleState);
                return;
            }
        }

        // Destroy breadcrumb if close enough
        if (targetBreadcrumb != null)
        {
            float distToCrumb = Vector2.Distance(enemy.GetMyPos(), targetBreadcrumb.position);

            if (distToCrumb < 0.2f)
            {
                targetBreadcrumb = targetBreadcrumb.next;

                if (targetBreadcrumb == null)
                {
                    enemy.ChangeState(enemy.idleState);
                    return;
                }
            }
        }

        Vector2 dir = (targetPos - enemy.GetMyPos()).normalized;
        enemy.Move(dir, enemy.Data.walkSpeed);

        footstepTimer -= Time.deltaTime;
        float speed = enemy.anim.GetFloat("speed");
        if (speed > 0.1f && footstepTimer <= 0f)
        {
            SoundManager.PlaySoundAtPosition(
                enemy.Data.soundFootSteps,
                enemy.transform.position,
                enemy.player,    
                10f        
            );            
            
            footstepTimer = footstepInterval;
        }
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