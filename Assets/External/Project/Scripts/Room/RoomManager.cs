using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;
    public Transform player;
    public float roomGapY = 20f;
    public Room room;
    public bool isOpen = false;
    public float moveDistanceY = 10f;
    [SerializeField] private SkillSelectorUI sui;

    private void Awake()
    {
        Instance = this;
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.LogWarning("플레이어 오브젝트가 없음.");
            }
        }
    }

    void Update()
    {
        // Room이 연결되어 있고 적이 전부 제거되면 자동으로 열림
        if (room != null && room.AreAllEnemiesCleared())
        {
            if (!isOpen)
            {
                isOpen = true;
                Debug.Log($"{gameObject.name} 문 열림 (Enemy 전멸)");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOpen && collision.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 newPos = rb.position;
                newPos.y += moveDistanceY;
                rb.MovePosition(newPos);
                Debug.Log("Player 다음 Room으로 이동!");
            }
        }
    }

    public void OnRoomCleared(int clearedRoomIndex)
    {
        MovePlayerToNextRoom();
    }

    private void MovePlayerToNextRoom()
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D> ();
        if (rb != null)
        {
            Vector2 newPos = rb.position;
            newPos.y += roomGapY;
            rb.MovePosition(newPos); // Rigidbody로 안전하게 이동
            Debug.Log($"Rigidbody로 Player 이동: {newPos}");
        }
        else
        {
            // 혹시 Rigidbody가 없으면 Transform으로 이동
            Vector3 newPosition = player.position;
            newPosition.y += roomGapY;
            player.position = newPosition;
            Debug.Log($"Transform으로 Player 이동: {player.position}");
        }
    }
}