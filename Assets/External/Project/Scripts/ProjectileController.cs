using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private LayerMask levelCollisionLayer; // 지형(벽 등) 충돌 판정용 레이어

    private RangeWeaponHandler rangeWeaponHandler; // 발사에 사용된 무기 정보 참조

    private float currentDuration; // 현재까지 살아있는 시간
    private Vector2 direction; // 발사 방향
    private bool isReady; // 발사 준비 완료 여부
    private Transform pivot; // 총알의 시각 회전을 위한 피벗

    private Rigidbody2D _rigidbody;
    private SpriteRenderer spriteRenderer;

    public bool fxOnDestory = true; // 충돌 시 이펙트를 생성할지 여부
    private int bounceCount = 0;
    private int maxBounces = 0;
    private float bounceMultiplier = 1f;
    private float damageValue;

    // 초기 컴포넌트 참조 설정
    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        pivot = transform.GetChild(0); // 피벗 오브젝트 (스프라이트 회전용)
    }

    private void Update()
    {
        if (!isReady)
        {
            return;
        }

        // 생존 시간 누적
        currentDuration += Time.deltaTime;

        // 설정된 지속 시간 초과 시 자동 파괴
        if (currentDuration > rangeWeaponHandler.Duration)
        {
            DestroyProjectile(transform.position, false);
        }

        // 물리 이동 처리 (방향 * 속도)
        _rigidbody.velocity = direction * rangeWeaponHandler.Speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (levelCollisionLayer.value == (levelCollisionLayer.value | (1 << collision.gameObject.layer)))
        {
            DestroyProjectile(collision.ClosestPoint(transform.position) - direction * .2f, fxOnDestory);
        }
        else if (rangeWeaponHandler.target.value == (rangeWeaponHandler.target.value | (1 << collision.gameObject.layer)))
        {
            // 데미지 적용을 위해 체력 시스템이 있는지 확인
            UnitController resourceController = collision.GetComponent<UnitController>();

            if (resourceController != null)
            {
                float finalDamage = rangeWeaponHandler.Power;

                if (rangeWeaponHandler.shooter != null &&
                    rangeWeaponHandler.shooter.TryGetComponent<Player>(out var player))
                {
                    // 멀티샷 데미지 감소
                    if (player.MultiShotCount > 0)
                        finalDamage *= player.MultiShotDamageMultiplier;
                        
                    // Rage 스킬 적용
                    if (player.IsRage)
                    {
                        float lostHpRatio = 1f - (player.CurrentHealth / player.Health);
                        float rageMultiplier = 1f + (lostHpRatio * 1.2f); // 잃은 체력 1% 당 1.2% 대미지 증가
                        finalDamage *= rageMultiplier;
                        Debug.Log($"Rage Skill 적용됨. {Mathf.RoundToInt(lostHpRatio * 100)}% 체력 손실 => x{rageMultiplier:F2} 대미지");
                    }

                    // DeadlyShot 적용
                    if (player.HasDeadlyShotEffect && !resourceController.IsBoss && !resourceController.HasDeadlyShotApplied)
                    {
                        resourceController.HasDeadlyShotApplied = true;

                        float ds = Random.value;

                        if (ds < 0.12f)
                        {
                            Debug.Log("DeadlyShot 성공.");
                            resourceController.ChangeHealth(-resourceController.CurrentHealth); // 즉사 로직
                            DestroyProjectile(collision.ClosestPoint(transform.position), fxOnDestory);
                            return;
                        }
                    }
                    // 크리티컬 적용
                    float criticalChance = player.Critical / 100f;

                    if (Random.value < criticalChance)
                    {
                        finalDamage *= player.CriticalDamage;
                        Debug.Log($"크리티컬 히트, 피해량: {finalDamage}.");
                    }
                }

                // 데미지 적용
                bool isDead = resourceController.ChangeHealth(-finalDamage);

                if (isDead && rangeWeaponHandler.shooter != null &&
                    rangeWeaponHandler.shooter.TryGetComponent<Player>(out player) &&
                    player.HasHealOnKillEffect)
                {
                    float healAmount = player.Health * 0.015f;
                    player.ChangeHealth(healAmount);
                    Debug.Log($"적 처치로 인한 체력 회복: {healAmount}");
                }

                // 넉백 설정이 되어 있다면 넉백도 적용
                if (rangeWeaponHandler.IsOnKnockback)
                {
                    UnitController controller = collision.GetComponent<UnitController>();
                    if (controller != null)
                    {
                        controller.ApplyKnockback(transform, rangeWeaponHandler.KnockbackPower, rangeWeaponHandler.KnockbackTime);
                    }
                }

                if (rangeWeaponHandler.shooter != null &&
                    rangeWeaponHandler.shooter.TryGetComponent<Player>(out Player ricochetPlayer) &&
                    ricochetPlayer.HasRicochetSkill &&
                    bounceCount < ricochetPlayer.RicochetMaxBounces)
                {
                    Transform nextTarget = FindNextTarget(resourceController.transform);

                    if (nextTarget != null)
                    {
                        Vector2 newDirection = (nextTarget.position - transform.position).normalized;
                        GameObject ricochet = Instantiate(gameObject, transform.position, Quaternion.identity);
                        var ricochetController = ricochet.GetComponent<ProjectileController>();
                        ricochetController.bounceCount = this.bounceCount + 1;
                        ricochetController.damageValue = this.damageValue * ricochetPlayer.RicochetDamageMultiplier;
                        ricochetController.Init(newDirection, rangeWeaponHandler);

                        Destroy(gameObject);
                        return;
                    }
                }
            }

            DestroyProjectile(collision.ClosestPoint(transform.position), fxOnDestory);
        }
    }

    private Transform FindNextTarget(Transform excludeTarget)
    {
        Collider2D[] candidates = Physics2D.OverlapCircleAll(transform.position, 5f, rangeWeaponHandler.target);

        Transform closest = null;
        float minDist = Mathf.Infinity;

        foreach (var col in candidates)
        {
            if (col.transform == excludeTarget) continue; // 이전 타겟 제외

            float dist = Vector2.Distance(transform.position, col.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = col.transform;
            }
        }

        return closest;
    }


    public void Init(Vector2 direction, RangeWeaponHandler weaponHandler)
    {
        rangeWeaponHandler = weaponHandler;

        this.direction = direction;
        currentDuration = 0;

        // 크기 및 색상 적용
        transform.localScale = Vector3.one * weaponHandler.BulletSize;
        spriteRenderer.color = weaponHandler.ProjectileColor;

        // 회전 방향 설정 (정면 방향 지정)
        transform.right = this.direction;

        // X 방향에 따라 스프라이트 위아래 반전 (좌우 발사 시)
        if (this.direction.x < 0)
            pivot.localRotation = Quaternion.Euler(180, 0, 0);
        else
            pivot.localRotation = Quaternion.Euler(0, 0, 0);

        damageValue = rangeWeaponHandler.Power;

        // 반동샷 세팅
        if (weaponHandler.shooter != null &&
            weaponHandler.shooter.TryGetComponent<Player>(out var player) &&
            player.HasRicochetSkill)
        {
            maxBounces = player.RicochetMaxBounces;
            bounceMultiplier = player.RicochetDamageMultiplier;
        }

        isReady = true;
    }

    // 투사체 파괴 함수
    private void DestroyProjectile(Vector3 position, bool createFx)
    {
        Destroy(this.gameObject);
    }
}