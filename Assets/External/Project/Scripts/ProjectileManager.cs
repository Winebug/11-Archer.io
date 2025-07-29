using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    private static ProjectileManager instance;
    public static ProjectileManager Instance { get { return instance; } }

    // �߻��� ����ü ������ �迭 (�Ѿ� ������)
    [SerializeField] private GameObject[] projectilePrefabs;

    private void Awake()
    {
        instance = this;
    }

    public void ShootBullet(RangeWeaponHandler rangeWeaponHandler, Vector2 startPostiion, Vector2 direction)
    {
        // �ش� ���⿡�� ����� ����ü ������ ��������
        GameObject origin = projectilePrefabs[rangeWeaponHandler.BulletIndex];

        // ������ ��ġ�� ����ü ����
        GameObject obj = Instantiate(origin, startPostiion, Quaternion.identity);

        // ����ü�� �ʱ� ���� ���� (����, ���� ������)
        ProjectileController projectileController = obj.GetComponent<ProjectileController>();
        projectileController.Init(direction, rangeWeaponHandler);
    }

}
