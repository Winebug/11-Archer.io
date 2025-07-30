using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("장애물 프리팹")]
    public GameObject[] Damageobstacle;
    public GameObject[] Nonobstacle;

    [Header("방 범위 설정")]
    public Vector2 minSpawnArea = new Vector2(-4f, -2f);
    public Vector2 maxSpawnArea = new Vector2(4f, 2f);

    [Header("갯수 범위")]
    public int minObstacles = 2;
    public int maxObstacles = 5;

    // Start 제거하고 수동 실행으로 변경
    public void SpawnObstacles()
    {
        int obstacleCount = Random.Range(minObstacles, maxObstacles + 1);

        for (int i = 0; i < obstacleCount; i++)
        {
            bool spawnDamage = Random.value > 0.5f;
            GameObject prefab = spawnDamage
                ? Damageobstacle[Random.Range(0, Damageobstacle.Length)]
                : Nonobstacle[Random.Range(0, Nonobstacle.Length)];

            float randomX = Random.Range(minSpawnArea.x, maxSpawnArea.x);
            float randomY = Random.Range(minSpawnArea.y, maxSpawnArea.y);
            Vector3 pos = new Vector3(randomX, randomY, 0f);

            GameObject gameObject = Instantiate(prefab,transform);
            gameObject.transform.localPosition = pos;

            //i*+20

            //Instantiate(prefab, pos, Quaternion.identity, transform);
            //Instantiate생성 -> world -> Local 좌표로 생성.
        }
    }
}