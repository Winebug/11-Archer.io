using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    [Header("Spawn Points")]
    public Transform[] obstacleSpawnPoints;
    public Transform[] enemySpawnPoints;
    public Transform[] itemSpawnPoints;

    [Header("Prefabs")]
    public GameObject[] obstaclePrefabs;
    public GameObject[] enemyPrefabs;
    public GameObject[] itemPrefabs;

    public void SpawnRandomContent()
    {
        // ��ֹ� ���� ����
        foreach (var point in obstacleSpawnPoints)
        {
            GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
            Instantiate(prefab, point.position, Quaternion.identity, transform);
        }

        // �� ���� ����
        foreach (var point in enemySpawnPoints)
        {
            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            Instantiate(prefab, point.position, Quaternion.identity, transform);
        }

        // ������ ���� ����
        foreach (var point in itemSpawnPoints)
        {
            GameObject prefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)];
            Instantiate(prefab, point.position, Quaternion.identity, transform);
        }
    }
}
