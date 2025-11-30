using UnityEngine;
using System.Collections;

public class PlayerTakeDamage : MonoBehaviour
{
    private PlayerStats playerStats;
    private Movements playerMov;

    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        playerMov = GetComponent<Movements>();
    }

    private void OnEnable()
    {
        EnemyAI.OnEnemyAttack += HandleEnemyAttack;
    }

    private void OnDisable()
    {
        EnemyAI.OnEnemyAttack -= HandleEnemyAttack;
    }

    private void HandleEnemyAttack(EnemyAI enemy)
    {
        if (enemy == null || playerStats == null) return;

        float distance = Vector2.Distance(transform.position, enemy.transform.position);

        if (distance <= enemy.Data.attackRange)
        {
            StartCoroutine(DamageCoroutine(enemy, 0.1f));
        }
    }

    private IEnumerator DamageCoroutine(EnemyAI enemy, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (enemy == null || playerStats == null) yield break;

        float distance = Vector2.Distance(transform.position, enemy.transform.position);
        if (distance <= enemy.Data.attackRange)
        {
            playerStats.TakeDamage((int)enemy.Data.attackDamage);
            playerMov.Hurt();
        }
    }
}
