using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool isOpen = false;
    public Animator animator;

    private RoomManager roomManager;

    private void Start()
    {
        // RoomManager 캐싱
        roomManager = FindObjectOfType<RoomManager>();
    }

    // Door 열기
    public void OpenDoor()
    {
        if (isOpen) return;
        isOpen = true;

        if (animator != null)
            animator.SetBool("Open", true);

        GetComponent<Collider2D>().isTrigger = true;
    }

    // Player가 닿으면 RoomManager의 GoToNextRoom 호출
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOpen && collision.CompareTag("Player") && roomManager != null)
        {
            if (roomManager.CurrentRoomIndex < roomManager.roomPrefabs.Length - 1)
            {
                roomManager.GoToNextRoom();
            }
            else
            {
                Debug.Log("BossRoom 이후 Door → Clear 연출 실행");
                roomManager.ShowClearScreen(); // BossRoom 클리어 처리
            }
        }
    }
}
