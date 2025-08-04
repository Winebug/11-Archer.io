using UnityEngine;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    public int roomIndex;
    private List<Enemy> enemies = new List<Enemy>();
    public Door door; // 현재 Room의 Door 참조

    void Start()
    {
        enemies.Clear();
        Enemy[] foundEnemies = GetComponentsInChildren<Enemy>(true);
        foreach (var enemy in foundEnemies)
        {
            enemy.SetRoom(this);
            enemies.Add(enemy);
        }
    }
    public bool AreAllEnemiesCleared()
    {
        return enemies.Count == 0;
    }
}
