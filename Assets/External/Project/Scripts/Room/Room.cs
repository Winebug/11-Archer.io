using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int roomIndex;
    private List<Enemy> enemies = new List<Enemy>();
    public Door door; // 현재 Room의 Door 참조

    void Start()
    {
        enemies.Clear();
        Enemy[] foundEnemies = GetComponentsInChildren<Enemy>(true);
        Debug.LogWarning($"[{gameObject.name}:{GetInstanceID()}] 발견된 Enemy: {foundEnemies.Length}마리");

        int i = 0;
        foreach (var enemy in foundEnemies)
        {
            enemy.SetRoom(this);
            enemies.Add(enemy);
            i++;
            Debug.Log($"적 {i}마리째 추가. 현재 적 {enemies.Count}");
        }

    }

    public bool AreAllEnemiesCleared()
    {
        // null 참조 제거
        enemies.RemoveAll(enemy => enemy == null);
        Debug.LogWarning($"[{gameObject.name}:{GetInstanceID()}] 남은 적: {enemies.Count}");
        return enemies.Count == 0;
    }
}
