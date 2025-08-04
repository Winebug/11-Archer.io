using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;

    public Transform player;                // Player Transform
    public Transform[] roomStartPositions;   // 각 Room 시작 위치 (Room1, Room2…)

    private void Awake()
    {
        Instance = this;
    }

    public void OnRoomCleared(int clearedRoomIndex)
    {
        int nextRoomIndex = clearedRoomIndex + 1;

        if (nextRoomIndex < roomStartPositions.Length)
        {
            MovePlayerToRoom(nextRoomIndex);
        }
    }

    private void MovePlayerToRoom(int roomIndex)
    {
        // Player 위치를 Room 시작 지점으로 순간 이동
        player.position = roomStartPositions[roomIndex].position;
    }
}
