using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    private GameObject player;

    [SerializeField] private Vector3 offset;

    [SerializeField] private float smoothSpeed = 5f;

    [SerializeField] public bool constrain = true;

    Vector3 desiredPos;

    private float minY = -4.8f;
    private float minX = -5.7f;
    private float maxX = 11.7f;

    private float minY2 = 12.5f;
    private float minX2 = 34f;
    private float maxY2 = 33f;
    private float maxX2 = 56f;

    private bool onlyZone2 = false;
    private void Start()
    {
        Initialize();

        if (SceneManager.GetActiveScene().name == "TraidingScene")
        {
            Camera.main.orthographicSize = 5f;
        }
    }

    public void CutSceneCamera()
    { 
        player = null;
        transform.position = new Vector3(0f, -2f, transform.position.z);
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
        if (player == null)
        {
                return; 
        }

        desiredPos = player.transform.position + offset;

        if (constrain)
        {
            Constrain();
        }

        if (SceneManager.GetActiveScene().name == "TraidingScene")
        {
            ConstrainTraiding();
        }

        if (SceneManager.GetActiveScene().name == "ForestDung")
        {
            ConstrainForest();
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

    private void ConstrainForest()
    {
        desiredPos.x = Mathf.Clamp(desiredPos.x, -12.3f, 40f);
        desiredPos.y = Mathf.Max(desiredPos.y, -4.5f);
    }

    private void ConstrainTraiding()
    {
        desiredPos.x = Mathf.Clamp(desiredPos.x, -12.5f, 14f);
        desiredPos.y = Mathf.Clamp(desiredPos.y, -7.5f, 7.5f);
    }
}
