using UnityEngine;

public class BossRoomTrigger : MonoBehaviour
{
    private CameraFollow cameraFollow;

    void Start()
    {
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cameraFollow.SetBossRoom(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cameraFollow.SetBossRoom(false);
        }
    }
}
