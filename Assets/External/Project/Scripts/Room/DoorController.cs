using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool isOpen = false;
    public Animator animator;

    private RoomManager roomManager;
    private bool playerInRange = false; // Player가 문 범위 안에 있는지 체크

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

    private void Update()
    {
        // Player가 범위 안에 있고 Enter를 누르면 다음 방 이동
        if (isOpen && playerInRange && Input.GetKeyDown(KeyCode.Return) && roomManager != null)
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

    // Player가 Trigger 범위에 들어오면
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;

            if (roomManager != null)
            {
                roomManager.GoToNextRoom();
            }
            else
            {
                Debug.LogError("RoomManager 참조가 없습니다!");
            }
        }
    }

    // Player가 Trigger 범위를 벗어나면
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("문에서 멀어짐 - Enter 키 무효");
        }
    }
}
