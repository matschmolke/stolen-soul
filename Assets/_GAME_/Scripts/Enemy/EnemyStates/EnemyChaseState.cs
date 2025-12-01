using UnityEngine;

public class EnemyChaseState : CharacterState
{
    private EnemyAI enemy;

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
        if (!enemy.CanSeePlayer())
        {
            enemy.ChangeState(enemy.idleState);
            return;
        }
        
        Debug.Log("Enemy sees player!");
        float distance = Vector2.Distance(enemy.GetMyPos(), enemy.GetPlayerPos());
        
        if (distance <= enemy.Data.attackRange)
        {
            enemy.ChangeState(enemy.attackState);
            return;
        }

        Vector2 dir = (enemy.GetPlayerPos() - enemy.GetMyPos()).normalized;
        enemy.Move(dir, enemy.Data.walkSpeed);
    }
}