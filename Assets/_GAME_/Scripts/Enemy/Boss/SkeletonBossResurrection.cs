using UnityEngine;

public class SkeletonBossResurrection : MonoBehaviour
{
    private EnemyAI boss;
    private Animator anim;

    private bool hasResurected = false;

    void Awake()
    {
        boss = GetComponent<EnemyAI>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasResurected) return;

        if (boss.CanSeePlayer())
        {
            anim.SetTrigger("hasResurrected");
        }
    }

    public void OnResurrectionEnd()
    {
        hasResurected = true;
        boss.currentState = State.Idle;
        boss.hasDied = false;
    }
}
