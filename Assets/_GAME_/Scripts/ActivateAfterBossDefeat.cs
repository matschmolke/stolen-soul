using UnityEngine;

public class ActivateAfterBossDefeat : MonoBehaviour
{
    public EnemyAI BossAI;

    private SceneTeleport teleport;

    public void Awake()
    {
        teleport = GetComponent<SceneTeleport>();
    }

    public void Update()
    {
        if (BossAI.isDead)
        {
            teleport.canTeleport = true;
        }
    }
}
