using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private Transform destination;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerTriggerCollider"))
        {
            SoundManager.PlaySound(SoundType.TELEPORT);
            var player = collision.transform.parent.gameObject;
            //collision.transform.position = destination.position;
            player.transform.position = destination.position;
        }
    }
}