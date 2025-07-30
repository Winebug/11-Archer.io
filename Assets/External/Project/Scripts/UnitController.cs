using UnityEngine;

// 모든 캐릭터의 기본 움직임, 회전, 넉백 처리를 담당하는 기반 클래스
public class UnitController : MonoBehaviour
{
    protected Rigidbody2D _rigidbody; // 이동을 위한 물리 컴포넌트

    protected AnimationHandler animationHandler;

    [SerializeField] private SpriteRenderer characterRenderer; // 좌우 반전을 위한 렌더러
    [SerializeField] private Transform weaponPivot; // 무기를 회전시킬 기준 위치

    [SerializeField] protected float healthChangeDelay = .5f; // 피해 후 무적 지속 시간

    protected Vector2 movementDirection = Vector2.zero; // 현재 이동 방향
    public Vector2 MovementDirection { get { return movementDirection; } }

    protected Vector2 lookDirection = Vector2.zero; // 현재 바라보는 방향
    public Vector2 LookDirection { get { return lookDirection; } }

    private Vector2 knockback = Vector2.zero; // 넉백 방향
    private float knockbackDuration = 0.0f; // 넉백 지속 시간

    [SerializeField] public WeaponHandler WeaponPrefab; // 장착할 무기 프리팹 (없으면 자식에서 찾아 사용)
    protected WeaponHandler weaponHandler; // 장착된 무기

    protected bool isAttacking; // 공격 중 여부
    protected float timeSinceLastAttack = float.MaxValue; // 마지막 공격 이후 경과 시간

    protected float timeSinceLastChange = float.MaxValue; // 마지막 체력 변경 이후 경과 시간

    public float CurrentHealth { get; private set; } // 현재 체력 (외부 접근만 허용)


    // 체력 (1 ~ 100 사이 값만 허용)
    [Range(1, 100)][SerializeField] private int health = 10;
    // 외부에서 접근 가능한 프로퍼티 (값 변경 시 자동으로 0~100 사이로 제한)
    public int Health
    {
        get => health;
        set => health = Mathf.Clamp(value, 0, 100);
    }

    // 이동 속도 (1f ~ 20f 사이 값만 허용)
    [Range(1f, 20f)][SerializeField] private float speed = 3;
    // 외부에서 접근 가능한 프로퍼티 (값 변경 시 0~20f로 제한)
    public float Speed
    {
        get => speed;
        set => speed = Mathf.Clamp(value, 0, 20);
    }


    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        animationHandler = GetComponent<AnimationHandler>();

        // 프리팹이 지정되어 있다면 생성해서 장착 위치에 부착
        if (WeaponPrefab != null)
            weaponHandler = Instantiate(WeaponPrefab, weaponPivot);
        else
            weaponHandler = GetComponentInChildren<WeaponHandler>(); // 이미 붙어 있는 무기 사용
    }

    protected virtual void Start()
    {
        CurrentHealth = Health;
    }

    protected virtual void Update()
    {
        HandleAction();

        // 자동 공격 확인용
        if (isAttacking == true)
        {
            Debug.Log("isAttacking true");
        }

        if (movementDirection.magnitude > 0)
        {
            Rotate(lookDirection);
        }
        HandleAttackDelay(); // 공격 입력 및 쿨타임 관리


    }

    protected virtual void FixedUpdate()
    {
        Movment(movementDirection);
        if (knockbackDuration > 0.0f)
        {
            knockbackDuration -= Time.fixedDeltaTime; // 넉백 시간 감소
        }
    }

    // 캐릭터, 적 핸들액션 오버라이드 받기
    protected virtual void HandleAction()
    {

    }


    // 캐릭터 이동
    private void Movment(Vector2 direction)
    {
        direction = direction * Speed; // 이동 속도

        // 넉백 중이면 이동 속도 감소 + 넉백 방향 적용
        if (knockbackDuration > 0.0f)
        {
            direction *= 0.2f; // 이동 속도 감소
            direction += knockback; // 넉백 방향 추가
        }

        // 실제 물리 이동
        _rigidbody.velocity = direction;

        // 이동 애니메이션 처리
        if (animationHandler != null)
            animationHandler.Move(direction);
    }

    // 스프라이트 좌우 반전(캐릭,무기)
    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bool isLeft = Mathf.Abs(rotZ) > 90f;

        // 스프라이트 좌우 반전
        characterRenderer.flipX = isLeft;

        if (weaponPivot != null)
        {
            // 무기 회전 처리
            weaponPivot.rotation = Quaternion.Euler(0, 0, rotZ);
        }

        // 무기도 함께 좌우 반전 처리
        weaponHandler?.Rotate(isLeft);
    }

    // 넉백
    public void ApplyKnockback(Transform other, float power, float duration)
    {
        knockbackDuration = duration;
        // 상대 방향을 반대로 밀어냄
        knockback = -(other.position - transform.position).normalized * power;
    }

    // 체력 변경 함수 (피해 or 회복)
    public bool ChangeHealth(float change)
    {
        // 변화 없거나 무적 상태면 무시
        if (change == 0 || timeSinceLastChange < healthChangeDelay)
        {
            return false;
        }

        timeSinceLastChange = 0f; // 다시 무적 시작

        // 체력 적용
        CurrentHealth += change;
        CurrentHealth = CurrentHealth > Health ? Health : CurrentHealth;
        CurrentHealth = CurrentHealth < 0 ? 0 : CurrentHealth;

        // 데미지일 경우 (음수)
        if (change < 0)
        {
            animationHandler.Damage(); // 맞는 애니메이션 실행

        }

        // 체력이 0 이하가 되면 사망 처리
        if (CurrentHealth <= 0f)
        {
            Death();
        }

        return true;
    }

    public virtual void Death()
    {
        // 움직임 정지
        _rigidbody.velocity = Vector3.zero;

        // 모든 SpriteRenderer의 투명도 낮춰서 죽은 효과 연출
        foreach (SpriteRenderer renderer in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            Color color = renderer.color;
            color.a = 0.3f;
            renderer.color = color;
        }

        // 모든 컴포넌트(스크립트 포함) 비활성화
        foreach (Behaviour component in transform.GetComponentsInChildren<Behaviour>())
        {
            component.enabled = false;
        }

        // 2초 후 오브젝트 파괴
        Destroy(gameObject, 2f);
    }

    private void HandleAttackDelay()
    {
        if (weaponHandler == null)
            return;

        // 공격 쿨다운 중이면 시간 누적
        if (timeSinceLastAttack <= weaponHandler.Delay)
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        // 공격 입력 중이고 쿨타임이 끝났으면 공격 실행
        if (isAttacking && timeSinceLastAttack > weaponHandler.Delay)
        {
            timeSinceLastAttack = 0;
            Attack(); // 실제 공격 실행
        }
    }

    protected virtual void Attack()
    {
        // 바라보는 방향이 있을 때만 공격
        if (lookDirection != Vector2.zero)
            weaponHandler?.Attack();
    }

    // 공격범위 시각화
    protected virtual void ShowAttackRange()
    {

    }
}
