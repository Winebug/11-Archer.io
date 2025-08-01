using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EnemySpawner : MonoBehaviour
{
    public List<Enemy> enemyList;

    [Header("스폰 기준점")]
    //일단 월드 좌표 기준으로 생성됩니다. 나중에 방 위치를 받아와야 합니다. 
    public Vector3 tempSpawnStandard;
    
    [Header("방 범위 설정")]
    public Vector2 minSpawnArea = new Vector2(-4f, -2f);
    public Vector2 maxSpawnArea = new Vector2(4f, 2f);

    // 시작하면 한마리 생성되게 했습니다. 나중에는 게임매니저로 옮기면 될 것 같습니다. 
    void Start()
    {
        SpawnRandomEnemy(1, tempSpawnStandard);
    }

    // 스페이스바 눌러도 생성됩니다. 테스트용으로, 나중에 지워주세요
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnRandomEnemy(1, tempSpawnStandard);
        }
    }

    void SpawnRandomEnemy(int enemyNumber, Vector3 SpanwStandardPoint)
    {
        for (int i = 0;  i < enemyNumber; i++)
        {
            Enemy chosenEnemy = enemyList[Random.Range(0, enemyList.Count)];
            Vector3 spawnPostition = PostionForSpawn(SpanwStandardPoint);
            Instantiate(chosenEnemy, spawnPostition, Quaternion.identity);
 
        }

    }

    Vector3 PostionForSpawn(Vector3 SpanwStandardPoint)
    {
        float randomX = Random.Range(minSpawnArea.x, maxSpawnArea.x);
        float randomY = Random.Range(minSpawnArea.y, maxSpawnArea.y);
        Vector3 pos = SpanwStandardPoint + new Vector3(randomX, randomY, 0f);

        return pos;
    }
}
