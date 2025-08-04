using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy[] enemyPrefabs;
    public int spawnCount = 3;
    public Vector2 spawnAreaSize = new Vector2(7f, 10f);
    public Room room; // 현재 Room 참조

    void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        if (enemyPrefabs.Length == 0) return;

        for (int i = 0; i < spawnCount; i++)
        {
            float randX = Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f);
            float randY = Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f);
            Vector2 spawnPos = (Vector2)transform.position + new Vector2(randX, randY);

            Enemy prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            Enemy enemy = Instantiate(prefab, spawnPos, Quaternion.identity, transform);

            // 🔹 Enemy에게 Room 정보 넘기기
            enemy.SetRoom(room);
        }
    }
}
