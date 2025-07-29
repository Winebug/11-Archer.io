using UnityEngine;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    [Header("�� ���� ����")]
    public GameObject roomPrefab;        // Level ������
    public Vector3 nextRoomOffset = new Vector3(0, 20, 0); // �� �� �Ÿ� (Y������ 20�� ����)

    [Header("�ʱ� ����")]
    public float spawnInterval = 10f;    // �� ���� �ֱ� (��)
    public int maxRooms = 10;            // �ִ� �� ����

    private Vector3 nextSpawnPos = Vector3.zero;
    private List<GameObject> rooms = new List<GameObject>();

    private float timer = 0f;
    private int roomCount = 0;

    void Update()
    {
        timer += Time.deltaTime;

        // 10�ʸ��� �� ����
        if (timer >= spawnInterval && roomCount < maxRooms)
        {
            SpawnRoom();
            timer = 0f;
        }
    }

    void SpawnRoom()
    {
        // ���ο� �� ����
        GameObject newRoom = Instantiate(roomPrefab, nextSpawnPos, Quaternion.identity);

        // RoomSpawner ����
        RoomSpawner spawner = newRoom.GetComponent<RoomSpawner>();
        if (spawner != null)
        {
            spawner.SpawnRandomContent();
        }

        rooms.Add(newRoom);
        roomCount++;

        // ���� �� ��ġ ������Ʈ
        nextSpawnPos += nextRoomOffset;

        Debug.Log($"�� {roomCount} ���� �Ϸ�! ��ġ: {nextSpawnPos}");
    }
}
