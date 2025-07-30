using UnityEngine;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    [Header("방 생성 설정")]
    public GameObject roomPrefab;        // Level 프리팹
    public Vector3 nextRoomOffset = new Vector3(0, 20, 0); // 방 간 거리 (Y축으로 20씩 위로)

    [Header("초기 설정")]
    public float spawnInterval = 10f;    // 방 생성 주기 (초)
    public int maxRooms = 10;            // 최대 방 개수

    private Vector3 nextSpawnPos = Vector3.zero;
    private List<GameObject> rooms = new List<GameObject>();

    private float timer = 0f;
    private int roomCount = 0;

    void Update()
    {
        timer += Time.deltaTime;

        // 10초마다 방 생성
        if (timer >= spawnInterval && roomCount < maxRooms)
        {
            SpawnRoom();
            timer = 0f;
        }
    }

    void SpawnRoom()
    {
        // 새로운 방 생성
        GameObject newRoom = Instantiate(roomPrefab, nextSpawnPos, Quaternion.identity);

        // RoomSpawner 실행
        RoomSpawner spawner = newRoom.GetComponent<RoomSpawner>();
        if (spawner != null)
        {
            spawner.SpawnRandomContent();
        }

        rooms.Add(newRoom);
        roomCount++;

        // 다음 방 위치 업데이트
        nextSpawnPos += nextRoomOffset;

        Debug.Log($"방 {roomCount} 생성 완료! 위치: {nextSpawnPos}");
    }
}
