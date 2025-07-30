using UnityEngine;

// 모든 캐릭?��?�� 기본 ???직임, ?��?��, ?���? 처리�? ?��?��?��?�� 기반 ?��?��?��
public class UnitController : MonoBehaviour
{
    protected Rigidbody2D _rigidbody; // ?��?��?�� ?��?�� 물리 컴포?��?��

    protected AnimationHandler animationHandler;

    [SerializeField] private SpriteRenderer characterRenderer; // 좌우 반전?�� ?��?�� ?��?��?��
    [SerializeField] private Transform weaponPivot; // 무기�? ?��?��?��?�� 기�?? ?���?

    [SerializeField] protected float healthChangeDelay = .5f; // ?��?�� ?�� 무적 �??�� ?���?


    protected Vector2 movementDirection = Vector2.zero; // ?��?�� ?��?�� 방향
    public Vector2 MovementDirection { get { return movementDirection; } }

    protected Vector2 lookDirection = Vector2.zero; // ?��?�� 바라보는 방향
    public Vector2 LookDirection { get { return lookDirection; } }

    private Vector2 knockback = Vector2.zero; // ?���? 방향
    private float knockbackDuration = 0.0f; // ?���? �??�� ?���?

    [SerializeField] public WeaponHandler WeaponPrefab; // ?��착할 무기 ?��리팹 (?��?���? ?��?��?��?�� 찾아 ?��?��)
    protected WeaponHandler weaponHandler; // ?��착된 무기

    protected bool isAttacking; // ���� �� ����
    protected float attackRange; // ���� ����
    private float timeSinceLastAttack = float.MaxValue; // ������ ���� ���� ��� �ð�

    protected float timeSinceLastChange = float.MaxValue; // 마�??�? 체력 �?�? ?��?�� 경과 ?���?

    public float CurrentHealth { get; private set; } // ?��?�� 체력 (?���? ?��근만 ?��?��)


    // 체력 (1 ~ 100 ?��?�� 값만 ?��?��)
    [Range(1, 100)][SerializeField] private int health = 10;
    // ?���??��?�� ?���? �??��?�� ?��로퍼?�� (�? �?�? ?�� ?��?��?���? 0~100 ?��?���? ?��?��)
    public int Health
    {
        get => health;
        set => health = Mathf.Clamp(value, 0, 100);
    }

    // ?��?�� ?��?�� (1f ~ 20f ?��?�� 값만 ?��?��)
    [Range(1f, 20f)][SerializeField] private float speed = 3;
    // ?���??��?�� ?���? �??��?�� ?��로퍼?�� (�? �?�? ?�� 0~20f�? ?��?��)
    public float Speed
    {
        get => speed;
        set => speed = Mathf.Clamp(value, 0, 20);
    }


    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        animationHandler = GetComponent<AnimationHandler>();

        // ?��리팹?�� �??��?��?�� ?��?���? ?��?��?��?�� ?���? ?��치에 �?�?
        if (WeaponPrefab != null)
            weaponHandler = Instantiate(WeaponPrefab, weaponPivot);
        else
            weaponHandler = GetComponentInChildren<WeaponHandler>(); // ?���? 붙어 ?��?�� 무기 ?��?��
    }

    protected virtual void Start()
    {
        CurrentHealth = Health;
    }

    protected virtual void Update()
    {
        HandleAction();

        // ?��?�� 공격 ?��?��?��
        if(isAttacking == true)
        {
            Debug.Log("isAttacking true");
        }

        if (movementDirection.magnitude > 0)
        {
            Rotate(lookDirection);
        }
        HandleAttackDelay(); // 공격 ?��?�� �? 쿨�???�� �?�?

        
    }

    protected virtual void FixedUpdate()
    {
        Movment(movementDirection);
        if (knockbackDuration > 0.0f)
        {
            knockbackDuration -= Time.fixedDeltaTime; // ?���? ?���? 감소
        }
    }

    // 캐릭?��, ?�� ?��?��?��?�� ?��버라?��?�� 받기
    protected virtual void HandleAction()
    {

    }


    // 캐릭?�� ?��?��
    private void Movment(Vector2 direction)
    {
        direction = direction * Speed; // ?��?�� ?��?��

        // ?���? 중이�? ?��?�� ?��?�� 감소 + ?���? 방향 ?��?��
        if (knockbackDuration > 0.0f)
        {
            direction *= 0.2f; // ?��?�� ?��?�� 감소
            direction += knockback; // ?���? 방향 추�??
        }

        // ?��?�� 물리 ?��?��
        _rigidbody.velocity = direction;

        // ?��?�� ?��?��메이?�� 처리
        if (animationHandler != null)
            animationHandler.Move(direction);
    }

    // ?��?��?��?��?�� 좌우 반전(캐릭,무기)
    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bool isLeft = Mathf.Abs(rotZ) > 90f;

        // ?��?��?��?��?�� 좌우 반전
        characterRenderer.flipX = isLeft;

        if (weaponPivot != null)
        {
            // 무기 ?��?�� 처리
            weaponPivot.rotation = Quaternion.Euler(0, 0, rotZ);
        }

        // 무기?�� ?���? 좌우 반전 처리
        weaponHandler?.Rotate(isLeft);
    }

    // ?���?
    public void ApplyKnockback(Transform other, float power, float duration)
    {
        knockbackDuration = duration;
        // ?��??? 방향?�� 반�??�? �??��?��
        knockback = -(other.position - transform.position).normalized * power;
    }

    // 체력 �?�? ?��?�� (?��?�� or ?���?)
    public bool ChangeHealth(float change)
    {
        // �??�� ?��거나 무적 ?��?���? 무시
        if (change == 0 || timeSinceLastChange < healthChangeDelay)
        {
            return false;
        }

        timeSinceLastChange = 0f; // ?��?�� 무적 ?��?��

        // 체력 ?��?��
        CurrentHealth += change;
        CurrentHealth = CurrentHealth > Health ? Health : CurrentHealth;
        CurrentHealth = CurrentHealth < 0 ? 0 : CurrentHealth;

        // ?��미�???�� 경우 (?��?��)
        if (change < 0)
        {
            animationHandler.Damage(); // 맞는 ?��?��메이?�� ?��?��

        }

        // 체력?�� 0 ?��?���? ?���? ?���? 처리
        if (CurrentHealth <= 0f)
        {
            Death();
        }

        return true;
    }

    public virtual void Death()
    {
        // ???직임 ?���?
        _rigidbody.velocity = Vector3.zero;

        // 모든 SpriteRenderer?�� ?��명도 ?��춰서 죽�?? ?���? ?���?
        foreach (SpriteRenderer renderer in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            Color color = renderer.color;
            color.a = 0.3f;
            renderer.color = color;
        }

        // 모든 컴포?��?��(?��?��립트 ?��?��) 비활?��?��
        foreach (Behaviour component in transform.GetComponentsInChildren<Behaviour>())
        {
            component.enabled = false;
        }

        // 2�? ?�� ?��브젝?�� ?���?
        Destroy(gameObject, 2f);
    }

    private void HandleAttackDelay()
    {
        if (weaponHandler == null)
            return;

        // 공격 쿨다?�� 중이�? ?���? ?��?��
        if (timeSinceLastAttack <= weaponHandler.Delay)
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        // 공격 ?��?�� 중이�? 쿨�???��?�� ?��?��?���? 공격 ?��?��
        if (isAttacking && timeSinceLastAttack > weaponHandler.Delay)
        {
            timeSinceLastAttack = 0;
            Debug.Log("player is attacking");
            Attack(); // ���� ���� ����
        }
    }

    protected virtual void Attack()
    {
        // 바라보는 방향?�� ?��?�� ?���? 공격
        if (lookDirection != Vector2.zero)
            weaponHandler?.Attack();
    }

}
