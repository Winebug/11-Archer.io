using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Enemy ownerEnemy;

    private void Start()
    {
        // 2초 후에 총알 자동 삭제
        Destroy(gameObject, 2f);
    }
    
    // Enemy 소유자를 할당받는 함수
    public void SetOwner(Enemy enemy)
    {
        ownerEnemy = enemy;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null && ownerEnemy != null && ownerEnemy.StatData != null)
            {
                float damage = ownerEnemy.StatData.attackPower;
                player.ChangeHealth(-damage);
            }
            Destroy(gameObject);
        }
    }
}
