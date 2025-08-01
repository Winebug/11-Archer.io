using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private Transform target;
    private void Start()
    {
        // target이 비어 있으면 자동으로 "Player" 레이어 또는 태그를 가진 오브젝트 찾아 할당
        if (target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                target = playerObj.transform;
            }
            else
            {
                Debug.LogWarning($"{name}: Player 태그를 가진 오브젝트를 찾을 수 없습니다.");
            }
        }
    }
    public void ShootBullet()
    {
        if (bulletPrefab == null || bulletSpawnPoint == null || target == null)
        {
            Debug.LogWarning("BulletShooter: 필드가 할당되지 않았습니다.");
            return;
        }

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        Vector2 direction = (target.position - bulletSpawnPoint.position).normalized;

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * bulletSpeed;
        }
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        Enemy owner = GetComponentInParent<Enemy>();
        if (bulletScript != null && owner != null)
        {
            bulletScript.SetOwner(owner);
        }
        else
        {
            Debug.LogWarning("BulletShooter: Bullet 또는 Enemy owner 설정 실패");
        }
    }
}
