using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    private static ProjectileManager instance;
    public static ProjectileManager Instance { get { return instance; } }

    // 발사할 투사체 프리팹 배열 (총알 종류별)
    [SerializeField] private GameObject[] projectilePrefabs;

    private void Awake()
    {
        instance = this;
    }

    public void ShootBullet(RangeWeaponHandler rangeWeaponHandler, Vector2 startPostiion, Vector2 direction)
    {
        // 해당 무기에서 사용할 투사체 프리팹 가져오기
        GameObject origin = projectilePrefabs[rangeWeaponHandler.BulletIndex];

        // 지정된 위치에 투사체 생성
        GameObject obj = Instantiate(origin, startPostiion, Quaternion.identity);

        rangeWeaponHandler.shooter = rangeWeaponHandler.Controller.gameObject;

        // 투사체에 초기 정보 전달 (방향, 무기 데이터)
        ProjectileController projectileController = obj.GetComponent<ProjectileController>();
        projectileController.Init(direction, rangeWeaponHandler);
    }

}
