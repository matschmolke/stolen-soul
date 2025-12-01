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

    private float minY2 = 12.5f;
    private float minX2 = 34f;
    private float maxY2 = 33f;
    private float maxX2 = 56f;

    private bool onlyZone2 = false;
    void Start()
    {
        transform.position = new Vector3(0f,-2f, transform.position.z);
    }

    public void Initialize()
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
        float x = desiredPos.x;
        float y = desiredPos.y;

        bool inZone1 =
            x >= minX && x <= maxX &&
            y >= minY;

        // STREFA 2
        bool inZone2 =
            x >= minX2 && x <= maxX2 &&
            y >= minY2 && y <= maxY2;


        if (inZone2 || onlyZone2)
        {
            Zone2();
            if (inZone2) onlyZone2 = true;
        }
        else
        {
            Zone1();
        }
    }

    private void Zone1()
    {
        desiredPos.y = Mathf.Max(desiredPos.y, minY);
        desiredPos.x = Mathf.Clamp(desiredPos.x, minX, maxX);
    }

    private void Zone2()
    {
        desiredPos.y = Mathf.Clamp(desiredPos.y, minY2, maxY2);
        desiredPos.x = Mathf.Clamp(desiredPos.x, minX2, maxX2);
    }
}
