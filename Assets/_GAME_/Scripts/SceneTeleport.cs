using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTeleport : MonoBehaviour
{
    public string targetScene;

    [Header("Set to True if location has no boss to kill")]
    public bool canTeleport = false;

    [Header("Set to true only on dungeon exit teleports.")]
    public bool changeLocationIndex = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerTriggerCollider") && canTeleport)
        {
            SoundManager.PlaySound(SoundType.TELEPORT);
            SceneManager.LoadScene(targetScene);
            SceneManager.LoadSceneAsync("mod1", LoadSceneMode.Additive);

            if (changeLocationIndex) LocationManager.LocationIndex++;
        }
    }
}
