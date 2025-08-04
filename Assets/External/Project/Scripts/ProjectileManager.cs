using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    private static ProjectileManager instance;
    public static ProjectileManager Instance { get { return instance; } }

    // 발사할 투사체 프리팹 배열 (총알 종류별)
    [SerializeField] private GameObject[] projectilePrefabs;

    // 총알이 맞았을 때 사용할 파티클 시스템
    [SerializeField] private ParticleSystem impactParticleSystem;

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
        projectileController.Init(direction, rangeWeaponHandler, this);
    }

    // 총알이 충돌했을 때, 해당 위치에 이펙트 파티클을 생성하는 함수
    public void CreateImpactParticlesAtPosition(Vector3 position, RangeWeaponHandler weaponHandler)
    {
        Debug.Log($"CreateImpactParticlesAtPosition: {position}, BulletSize: {weaponHandler.BulletSize}");
        // 파티클 위치를 충돌 지점으로 이동
        impactParticleSystem.transform.position = position;
        // Burst 설정: 파편 수를 총알 크기에 따라 조절
        ParticleSystem.EmissionModule em = impactParticleSystem.emission;
        em.SetBurst(0, new ParticleSystem.Burst(0, Mathf.Ceil(weaponHandler.BulletSize * 5)));
        // 총알 크기에 따라 파편 속도도 증가시킴
        ParticleSystem.MainModule mainModule = impactParticleSystem.main;
        mainModule.startSpeedMultiplier = weaponHandler.BulletSize * 10f;
        // 파티클 재생
        impactParticleSystem.Play();
    }
}
