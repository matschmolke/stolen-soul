public class EnemyIdleState : CharacterState
{
    private EnemyAI enemy;

    public EnemyIdleState(EnemyAI enemy) : base(enemy)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        enemy.anim.SetFloat("speed", 0);
    }

    public override void Update()
    {
        if (enemy.CanSeePlayer())
        {
            enemy.ChangeState(enemy.chaseState);
        }
    }
}