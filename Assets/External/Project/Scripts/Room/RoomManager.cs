using UnityEngine;
using System.Collections;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;

    [Header("방 Prefab 리스트 (Room1~BossRoom1 순서대로 등록)")]
    public GameObject[] roomPrefabs;
    public Vector3 roomOffset = new Vector3(0, 20, 0);

    private int currentRoomIndex = 0;
    private GameObject currentRoom;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterEnemy(GameObject enemy)
    {
        Debug.Log($"Enemy 등록됨: {enemy.name}");
    }
    void Start()
    {
        SpawnRoom(0); // 첫 방 생성
    }

    public void OnRoomCleared(Room clearedRoom)
    {
        // 현재 방이 클리어된 경우만 다음 방으로 이동
        if (clearedRoom.roomIndex == currentRoomIndex)
        {
            Debug.Log($"Room {clearedRoom.roomIndex} 클리어됨 → 다음 방 이동");
            StartCoroutine(GoToNextRoomCoroutine());
        }
    }

    public void SpawnRoom(int index)
    {
        Vector3 spawnPos;

        // 첫 방이면 시작 위치 고정
        if (index == 0)
        {
            spawnPos = Vector3.zero;
        }
        else
        {
            // 이전 방의 Y 위치 + 오프셋
            spawnPos = currentRoom != null
                ? currentRoom.transform.position + roomOffset
                : Vector3.zero;
        }

        // 새 방 생성
        GameObject newRoom = Instantiate(roomPrefabs[index], spawnPos, Quaternion.identity);
        currentRoom = newRoom;
        currentRoomIndex = index;
    }

    private IEnumerator GoToNextRoomCoroutine()
    {
        // 현재 방 삭제
        if (currentRoom != null)
        {
            Destroy(currentRoom);
        }

        yield return null; // 다음 프레임까지 대기 후 생성

        // 다음 방 인덱스 증가
        currentRoomIndex++;
        if (currentRoomIndex < roomPrefabs.Length)
        {
            SpawnRoom(currentRoomIndex);
        }
        else
        {
            Debug.Log("모든 방 클리어!");
        }
    }
}
