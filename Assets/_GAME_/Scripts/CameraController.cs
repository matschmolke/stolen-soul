using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    private GameObject player;

    [SerializeField] private Vector3 offset;

    [SerializeField] private float smoothSpeed = 5f;

    [SerializeField] private bool constrain = true;

    Vector3 desiredPos;

    private float minY = -4.8f;
    private float minX = -5.7f;
    private float maxX = 11.7f;

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

        desiredPos = player.transform.position + offset;

        if (constrain)
        {
            Constrain();
        }

        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Constrain()
    {
        desiredPos.y = Mathf.Max(desiredPos.y, minY);
        desiredPos.x = Mathf.Clamp(desiredPos.x, minX, maxX);
    }
}
