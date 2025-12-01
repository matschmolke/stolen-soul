using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = transform.position;
        }
    }
}
