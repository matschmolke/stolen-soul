using UnityEngine;
using System.Collections;

public class PlayerTakeDamage : MonoBehaviour
{
    private PlayerStats playerStats;
    private Movements playerMov;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerStats = player.GetComponent<PlayerStats>();
            playerMov = player.GetComponent<Movements>();
        }
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
        if (playerStats == null) return;

        float distance = Vector2.Distance(transform.position, enemy.transform.position);

        if (distance <= enemy.Data.attackRange)
        {
           
            DealDamageWithDelay(enemy, 0.4f); 
        }
    }

    //solution to problem - Animation of the player "hurt" is faster than Enemy "Attack"
    private void DealDamageWithDelay(EnemyAI enemy, float delay)
    {
        StartCoroutine(DamageCoroutine(enemy, delay));
    }

    private IEnumerator DamageCoroutine(EnemyAI enemy, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (playerStats == null || playerMov == null || enemy == null) yield break;

        float distance = Vector2.Distance(transform.position, enemy.transform.position);
        if (distance <= enemy.Data.attackRange)
        {
            playerStats.TakeDamage((int)enemy.Data.attackDamage);
            playerMov.Hurt();
        }
    }
}
