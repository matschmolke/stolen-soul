using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameScene : MonoBehaviour
{
    public EnemyAI endBoss;
    
    void Update()
    {
        if(endBoss.currentHealth <= 0)
        {
            SceneManager.LoadScene("EndScene");
        }
    }
}
