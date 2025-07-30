using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private Transform target;

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
    }
}
