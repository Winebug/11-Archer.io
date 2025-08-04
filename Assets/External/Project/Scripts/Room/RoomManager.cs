using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;
    public Transform player;
    public float roomGapY = 20f;

    private void Awake()
    {
        Instance = this;
    }

    public void OnRoomCleared(int clearedRoomIndex)
    {
        MovePlayerToNextRoom();
    }

    private void MovePlayerToNextRoom()
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 newPos = rb.position;
            newPos.y += roomGapY;
            rb.MovePosition(newPos); // Rigidbody로 안전하게 이동
            Debug.Log($"✅ Rigidbody로 Player 이동: {newPos}");
        }
        else
        {
            // 혹시 Rigidbody가 없으면 Transform으로 이동
            Vector3 newPosition = player.position;
            newPosition.y += roomGapY;
            player.position = newPosition;
            Debug.Log($"✅ Transform으로 Player 이동: {player.position}");
        }
    }
}