using UnityEngine;

// 紐⑤뱺 罹먮┃?꽣?쓽 湲곕낯 ???吏곸엫, ?쉶?쟾, ?꼮諛? 泥섎━瑜? ?떞?떦?븯?뒗 湲곕컲 ?겢?옒?뒪
public class UnitController : MonoBehaviour
{
    protected Rigidbody2D _rigidbody; // ?씠?룞?쓣 ?쐞?븳 臾쇰━ 而댄룷?꼳?듃

    protected AnimationHandler animationHandler;

    [SerializeField] private SpriteRenderer characterRenderer; // 醫뚯슦 諛섏쟾?쓣 ?쐞?븳 ?젋?뜑?윭
    [SerializeField] private Transform weaponPivot; // 臾닿린瑜? ?쉶?쟾?떆?궗 湲곗?? ?쐞移?

    [SerializeField] protected float healthChangeDelay = .5f; // ?뵾?빐 ?썑 臾댁쟻 吏??냽 ?떆媛?


    protected Vector2 movementDirection = Vector2.zero; // ?쁽?옱 ?씠?룞 諛⑺뼢
    public Vector2 MovementDirection { get { return movementDirection; } }

    protected Vector2 lookDirection = Vector2.zero; // ?쁽?옱 諛붾씪蹂대뒗 諛⑺뼢
    public Vector2 LookDirection { get { return lookDirection; } }

    private Vector2 knockback = Vector2.zero; // ?꼮諛? 諛⑺뼢
    private float knockbackDuration = 0.0f; // ?꼮諛? 吏??냽 ?떆媛?

    [SerializeField] public WeaponHandler WeaponPrefab; // ?옣李⑺븷 臾닿린 ?봽由ы뙶 (?뾾?쑝硫? ?옄?떇?뿉?꽌 李얠븘 ?궗?슜)
    protected WeaponHandler weaponHandler; // ?옣李⑸맂 臾닿린

    protected bool isAttacking; // 공격 중 여부
    protected float attackRange; // 공격 범위
    private float timeSinceLastAttack = float.MaxValue; // 마지막 공격 이후 경과 시간

    protected float timeSinceLastChange = float.MaxValue; // 留덉??留? 泥대젰 蹂?寃? ?씠?썑 寃쎄낵 ?떆媛?

    public float CurrentHealth { get; private set; } // ?쁽?옱 泥대젰 (?쇅遺? ?젒洹쇰쭔 ?뿀?슜)

    [Range(1, 100)][SerializeField] private int health = 10;

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
    protected float attackRange;
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


    // ?씠?룞 ?냽?룄 (1f ~ 20f ?궗?씠 媛믩쭔 ?뿀?슜)
    [Range(1f, 20f)][SerializeField] private float speed = 3;
    // ?쇅遺??뿉?꽌 ?젒洹? 媛??뒫?븳 ?봽濡쒗띁?떚 (媛? 蹂?寃? ?떆 0~20f濡? ?젣?븳)

    public float Speed
    {
        get => speed;
        set => speed = Mathf.Clamp(value, 0, 20);
    }


    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        animationHandler = GetComponent<AnimationHandler>();
     // ?봽由ы뙶?씠 吏??젙?릺?뼱 ?엳?떎硫? ?깮?꽦?빐?꽌 ?옣李? ?쐞移섏뿉 遺?李?
        if (WeaponPrefab != null)
            weaponHandler = Instantiate(WeaponPrefab, weaponPivot);
        else
            weaponHandler = GetComponentInChildren<WeaponHandler>(); // ?씠誘? 遺숈뼱 ?엳?뒗 臾닿린 ?궗?슜

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


        // ?옄?룞 怨듦꺽 ?솗?씤?슜

        // 자동 공격 확인용

        if(isAttacking == true)
        {
            //Debug.Log("isAttacking true");
        }

        if (movementDirection.magnitude > 0)
        {
            Rotate(lookDirection);
        }
        HandleAttackDelay(); // 怨듦꺽 ?엯?젰 諛? 荑⑦???엫 愿?由?



        
    }

    protected virtual void FixedUpdate()
    {
        Movment(movementDirection);
        if (knockbackDuration > 0.0f)
        {
            knockbackDuration -= Time.fixedDeltaTime; // ?꼮諛? ?떆媛? 媛먯냼
        }
    }

    // 罹먮┃?꽣, ?쟻 ?빖?뱾?븸?뀡 ?삤踰꾨씪?씠?뱶 諛쏄린
            knockbackDuration -= Time.fixedDeltaTime; // 넉백 시간 감소
        }
    }

    // 캐릭터, 적 핸들액션 오버라이드 받기
    protected virtual void HandleAction()
    {

    }


    // 罹먮┃?꽣 ?씠?룞
    private void Movment(Vector2 direction)
    {
        direction = direction * Speed; // ?씠?룞 ?냽?룄

        // ?꼮諛? 以묒씠硫? ?씠?룞 ?냽?룄 媛먯냼 + ?꼮諛? 諛⑺뼢 ?쟻?슜
        if (knockbackDuration > 0.0f)
        {
            direction *= 0.2f; // ?씠?룞 ?냽?룄 媛먯냼
            direction += knockback; // ?꼮諛? 諛⑺뼢 異붽??
        }

        // ?떎?젣 臾쇰━ ?씠?룞
        _rigidbody.velocity = direction;

        // ?씠?룞 ?븷?땲硫붿씠?뀡 泥섎━
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
    // ?뒪?봽?씪?씠?듃 醫뚯슦 諛섏쟾(罹먮┃,臾닿린)
    // 스프라이트 좌우 반전(캐릭,무기)
    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bool isLeft = Mathf.Abs(rotZ) > 90f;
        characterRenderer.flipX = isLeft;

        if (weaponPivot != null)
        {
            // 臾닿린 ?쉶?쟾 泥섎━
            weaponPivot.rotation = Quaternion.Euler(0, 0, rotZ);
        }

        // 臾닿린?룄 ?븿猿? 醫뚯슦 諛섏쟾 泥섎━
        weaponHandler?.Rotate(isLeft);
    }

    // ?꼮諛?
    public void ApplyKnockback(Transform other, float power, float duration)
    {
        knockbackDuration = duration;
        // ?긽??? 諛⑺뼢?쓣 諛섎??濡? 諛??뼱?깂
        knockback = -(other.position - transform.position).normalized * power;
    }

    // 泥대젰 蹂?寃? ?븿?닔 (?뵾?빐 or ?쉶蹂?)
    public bool ChangeHealth(float change)
    {
        // 蹂??솕 ?뾾嫄곕굹 臾댁쟻 ?긽?깭硫? 臾댁떆
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
        timeSinceLastChange = 0f; // ?떎?떆 臾댁쟻 ?떆?옉

        // 泥대젰 ?쟻?슜
        timeSinceLastChange = 0f; // 다시 무적 시작

        // 체력 적용
        CurrentHealth += change;
        CurrentHealth = CurrentHealth > Health ? Health : CurrentHealth;
        CurrentHealth = CurrentHealth < 0 ? 0 : CurrentHealth;

        // ?뜲誘몄???씪 寃쎌슦 (?쓬?닔)
        if (change < 0)
        {
            animationHandler.Damage(); // 留욌뒗 ?븷?땲硫붿씠?뀡 ?떎?뻾

        }

        // 泥대젰?씠 0 ?씠?븯媛? ?릺硫? ?궗留? 泥섎━
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
        // ???吏곸엫 ?젙吏?
        _rigidbody.velocity = Vector3.zero;

        // 紐⑤뱺 SpriteRenderer?쓽 ?닾紐낅룄 ?궙異곗꽌 二쎌?? ?슚怨? ?뿰異?
        // 움직임 정지
        _rigidbody.velocity = Vector3.zero;

        // 모든 SpriteRenderer의 투명도 낮춰서 죽은 효과 연출
        foreach (SpriteRenderer renderer in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            Color color = renderer.color;
            color.a = 0.3f;
            renderer.color = color;
        }

        // 紐⑤뱺 而댄룷?꼳?듃(?뒪?겕由쏀듃 ?룷?븿) 鍮꾪솢?꽦?솕
        // 모든 컴포넌트(스크립트 포함) 비활성화
        foreach (Behaviour component in transform.GetComponentsInChildren<Behaviour>())
        {
            component.enabled = false;
        }

        // 2珥? ?썑 ?삤釉뚯젥?듃 ?뙆愿?
        // 2초 후 오브젝트 파괴
        Destroy(gameObject, 2f);
    }

    private void HandleAttackDelay()
    {
        if (weaponHandler == null)
            return;

        // 怨듦꺽 荑⑤떎?슫 以묒씠硫? ?떆媛? ?늻?쟻
        // 공격 쿨다운 중이면 시간 누적
        if (timeSinceLastAttack <= weaponHandler.Delay)
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        // 怨듦꺽 ?엯?젰 以묒씠怨? 荑⑦???엫?씠 ?걹?궗?쑝硫? 怨듦꺽 ?떎?뻾
        if (isAttacking && timeSinceLastAttack > weaponHandler.Delay)
        {
            timeSinceLastAttack = 0;
            Debug.Log("player is attacking");
        // 공격 입력 중이고 쿨타임이 끝났으면 공격 실행
        if (isAttacking && timeSinceLastAttack > weaponHandler.Delay)
        {
            timeSinceLastAttack = 0;
            Attack(); // 실제 공격 실행
        }
    }

    protected virtual void Attack()
    {
        // 諛붾씪蹂대뒗 諛⑺뼢?씠 ?엳?쓣 ?븣留? 怨듦꺽
        // 바라보는 방향이 있을 때만 공격
        if (lookDirection != Vector2.zero)
            weaponHandler?.Attack();
    }

}
