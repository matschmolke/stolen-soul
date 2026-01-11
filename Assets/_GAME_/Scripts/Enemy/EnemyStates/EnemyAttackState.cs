using UnityEngine;

public class EnemyAttackState : CharacterState
{
    private EnemyAI enemy;

    public EnemyAttackState(EnemyAI enemy) : base(enemy)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        enemy.canAttack = false;
        enemy.anim.SetTrigger("isAttacking");
        SoundManager.PlaySound(enemy.Data.soundAttach);

        enemy.rb.linearVelocity = Vector2.zero;
    }


    public override void Update()
    {
        float dist = Vector2.Distance(enemy.GetMyPos(), enemy.GetPlayerPos());
        if (dist > enemy.Data.attackRange * 1.2f)
        {
            enemy.ChangeState(enemy.chaseState);
            return;
        }

        if (enemy.canAttack)
        {
            enemy.TriggerAttack();

            enemy.ChangeState(enemy.chaseState);
        }
    }

}