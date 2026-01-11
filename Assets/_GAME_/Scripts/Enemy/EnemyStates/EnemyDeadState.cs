using System.Collections;
using UnityEngine;

public class EnemyDeadState : CharacterState
{
    private EnemyAI enemy;
    public float deathBlinkDuration = 0.8f;
    public float deathBlinkFrequency = 0.1f;

    public EnemyDeadState(EnemyAI enemy) : base(enemy)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        enemy.isDead = true;
        enemy.anim.SetTrigger("isDead");
        
        SoundManager.PlaySound(SoundType.SKELETON_DEATH, 0.5f);
        enemy.StartCoroutine(DeathCoroutine());
    }
    
    public IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(3);

        float timer = 0f;
        bool visible = true;

        while (timer < deathBlinkDuration)
        {
            visible = !visible;
            enemy.spriteRenderer.enabled = visible;

            timer += deathBlinkFrequency;
            yield return new WaitForSeconds(deathBlinkFrequency);
        }

        enemy.spriteRenderer.enabled = false;

        enemy.lootBag.DropLoot(enemy.transform.position);

        Object.Destroy(enemy.gameObject);
    }
}