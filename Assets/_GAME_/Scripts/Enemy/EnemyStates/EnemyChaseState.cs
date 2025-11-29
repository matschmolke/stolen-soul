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
        character.moveSpeed = enemy.Data.runSpeed;
    }

    public override void Update()
    {
        if (!enemy.CanSeePlayer())
        {
            enemy.ChangeState(enemy.idleState);
            return;
        }

        float distance = Vector2.Distance(enemy.GetMyPos(), enemy.GetPlayerPos());

        if (distance <= enemy.Data.attackRange)
        {
            enemy.ChangeState(enemy.attackState);
            return;
        }

        Vector2 dir = (enemy.GetPlayerPos() - enemy.GetMyPos()).normalized;
        character.Move(dir);
    }
}