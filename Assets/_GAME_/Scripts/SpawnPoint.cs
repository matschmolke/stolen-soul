using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private void Start()
    {
        GameObject playerTriggerCollider = GameObject.FindWithTag("PlayerTriggerCollider");
        GameObject player = playerTriggerCollider.transform.parent.gameObject;
        if (player != null)
        {
            player.transform.position = transform.position;

            Camera.main.GetComponent<CameraController>().constrain = false;
            Camera.main.GetComponent<CameraController>().Initialize();
        }
    }
}
