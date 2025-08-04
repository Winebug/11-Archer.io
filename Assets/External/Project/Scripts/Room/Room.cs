using UnityEngine;

public class Room : MonoBehaviour
{
    public int roomIndex;

    void Start()
    {
        EnemySpawner spawner = GetComponentInChildren<EnemySpawner>();
        if (spawner != null)
        {
            spawner.room = this; // EnemySpawner가 Room을 알도록 전달
        }
    }

    public void OnEnemyDeath(Enemy enemy)
    {
        Debug.Log($"Room {roomIndex}에서 {enemy.name} 사망");
        if (GetComponentsInChildren<Enemy>().Length == 0)
        {
            Debug.Log($"Room {roomIndex} 클리어 → 다음 방 이동");
            RoomManager.Instance.OnRoomCleared(this);
        }
    }
}