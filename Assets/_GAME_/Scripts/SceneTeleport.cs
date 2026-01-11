using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTeleport : MonoBehaviour
{
    [SerializeField] private string targetScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerTriggerCollider"))
        {
            SoundManager.PlaySound(SoundType.TELEPORT);
            SceneManager.LoadScene(targetScene);
            SceneManager.LoadSceneAsync("mod1", LoadSceneMode.Additive);
        }
    }
}
