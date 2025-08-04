using UnityEngine;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    public int roomIndex;
    private List<Enemy> enemies = new List<Enemy>();

    void OnEnable()
    {
        // Room 활성화될 때 Enemy 다시 등록
        enemies.Clear();
        Enemy[] foundEnemies = GetComponentsInChildren<Enemy>(true);
        foreach (var enemy in foundEnemies)
        {
            enemy.SetRoom(this);
            enemies.Add(enemy);
        }
    }

    public void OnEnemyDeath(Enemy enemy)
    {
        enemies.Remove(enemy);
        if (enemies.Count == 0)
        {
            RoomManager.Instance.OnRoomCleared(roomIndex);
        }
    }
}
