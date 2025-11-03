using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    private GameObject player;
    public Vector3 offset;
    public float smoothSpeed = 5f;

    void Start()
    {
        TryFindPlayer();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "PlayerScene")
        {
            TryFindPlayer();
        }
    }

    void TryFindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
    }

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 desiredPos = player.transform.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

