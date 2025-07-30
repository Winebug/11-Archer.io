using UnityEngine;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    [Header("방 프리팹 설정")]
    public GameObject FirstRoom;   // 첫 방 전용
    public GameObject Level;  // 이후 방 전용

    [Header("방 생성 설정")]
    public Vector3 nextRoomOffset = new Vector3(0, 20, 0);
    public float spawnInterval = 10f;
    public int maxRooms = 10;

    private Vector3 nextSpawnPos = Vector3.zero;
    private List<GameObject> rooms = new List<GameObject>();
    private float timer = 0f;
    private int roomCount = 0;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval && roomCount < maxRooms)
        {
            SpawnRoom();
            timer = 0f;
        }
    }

    void SpawnRoom()
    {
        GameObject newRoom;

        // roomCount를 먼저 올려놓고 판단
        roomCount++;

        if (roomCount == 1)
        {
            Debug.Log(">>> 첫 번째 방 생성");
            newRoom = Instantiate(FirstRoom, nextSpawnPos, Quaternion.identity);
        }
        else
        {
            Debug.Log($">>> {roomCount}번째 방 생성 (Spawner 실행)");
            newRoom = Instantiate(Level, nextSpawnPos, Quaternion.identity);

            ObstacleSpawner spawner = newRoom.GetComponentInChildren<ObstacleSpawner>();
            if (spawner != null)
                spawner.SpawnObstacles();
        }

        rooms.Add(newRoom);
        nextSpawnPos += nextRoomOffset;
    }
}
