using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool isOpen = false;
    public Animator animator;

    // Door 열기
    public void OpenDoor()
    {
        if (isOpen) return;
        isOpen = true;

        if (animator != null)
            animator.SetBool("Open", true);

        // Collider를 Trigger로 변경 → Player 통과 가능
        GetComponent<Collider2D>().isTrigger = true;
    }

    // Player가 닿으면 RoomManager의 SpawnRoom 호출
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOpen && collision.CompareTag("Player"))
        {
            // RoomManager 찾아서 다음 방 생성
            RoomManager roomManager = FindObjectOfType<RoomManager>();
            if (roomManager != null)
                roomManager.SpawnRoom();
        }
    }
}
