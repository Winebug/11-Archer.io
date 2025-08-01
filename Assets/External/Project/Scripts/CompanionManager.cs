using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CompanionManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> companionPrefabs; // 생성할 컴패니언 프리팹 리스트

    [SerializeField]
    private List<Rect> spawnAreas; // 컴패니언을 생성할 영역 리스트

    [SerializeField]
    private Color gizmoColor = new Color(1, 0, 0, 0.3f); // 기즈모 색상

    private List<Companion> activeEnemies = new List<Companion>(); // 현재 활성화된 컴패니언들

    GameManager gameManager;

    public void Init(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    // 컴패니언 하나를 위치에 생성
    public void SpawnCompanion()
    {
        if (companionPrefabs.Count == 0 || spawnAreas.Count == 0)
        {
            Debug.LogWarning("Companion Prefabs 또는 Spawn Areas가 설정되지 않았습니다.");
            return;
        }

        // 랜덤한 컴패니언 프리팹 선택
        GameObject randomPrefab = companionPrefabs[Random.Range(0, companionPrefabs.Count)];

        // 랜덤한 영역 선택
        Rect randomArea = spawnAreas[Random.Range(0, spawnAreas.Count)];

        // Rect 영역 내부의 랜덤 위치 계산
        Vector2 randomPosition = new Vector2(
            Random.Range(randomArea.xMin, randomArea.xMax),
            Random.Range(randomArea.yMin, randomArea.yMax)
        );

        // 컴패니언 생성 및 리스트에 추가
        GameObject spawnedCompanion = Instantiate(randomPrefab, new Vector3(randomPosition.x, randomPosition.y), Quaternion.identity);
        Companion companion = spawnedCompanion.GetComponent<Companion>();

        activeEnemies.Add(companion);
    }

    // 기즈모를 그려 영역을 시각화 (선택된 경우에만 표시)
    private void OnDrawGizmosSelected()
    {
        if (spawnAreas == null) return;

        Gizmos.color = gizmoColor;
        foreach (var area in spawnAreas)
        {
            Vector3 center = new Vector3(area.x + area.width / 2, area.y + area.height / 2);
            Vector3 size = new Vector3(area.width, area.height);
            Gizmos.DrawCube(center, size);
        }
    }
}
