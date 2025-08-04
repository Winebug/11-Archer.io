using UnityEngine;

public class Door : MonoBehaviour
{
    public Room room; // 이 Door가 속한 Room 참조
    public bool isOpen = false;
    public float moveDistanceY = 10f;
    

    void Update()
    {
        // Room이 연결되어 있고 적이 전부 제거되면 자동으로 열림
        if (room != null && room.AreAllEnemiesCleared())
        {
            if (!isOpen)
            {
                isOpen = true;
                Debug.Log($"{gameObject.name} 문 열림 (Enemy 전멸)");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOpen && collision.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 newPos = rb.position;
                newPos.y += moveDistanceY;
                rb.MovePosition(newPos);
                Debug.Log("Player 다음 Room으로 이동!");
            }
        }
    }
}