using UnityEngine;

// ��� ĳ������ �⺻ ������, ȸ��, �˹� ó���� ����ϴ� ��� Ŭ����
public class UnitController : MonoBehaviour
{
    protected Rigidbody2D _rigidbody; // �̵��� ���� ���� ������Ʈ

    protected AnimationHandler animationHandler;

    [SerializeField] private SpriteRenderer characterRenderer; // �¿� ������ ���� ������
    [SerializeField] private Transform weaponPivot; // ���⸦ ȸ����ų ���� ��ġ

    [SerializeField] private float healthChangeDelay = .5f; // ���� �� ���� ���� �ð�


    protected Vector2 movementDirection = Vector2.zero; // ���� �̵� ����
    public Vector2 MovementDirection { get { return movementDirection; } }

    protected Vector2 lookDirection = Vector2.zero; // ���� �ٶ󺸴� ����
    public Vector2 LookDirection { get { return lookDirection; } }

    private Vector2 knockback = Vector2.zero; // �˹� ����
    private float knockbackDuration = 0.0f; // �˹� ���� �ð�

    [SerializeField] public WeaponHandler WeaponPrefab; // ������ ���� ������ (������ �ڽĿ��� ã�� ���)
    protected WeaponHandler weaponHandler; // ������ ����

    protected bool isAttacking; // ���� �� ����
    protected float attackRange; // ���� ����
    private float timeSinceLastAttack = float.MaxValue; // ������ ���� ���� ��� �ð�

    private float timeSinceLastChange = float.MaxValue; // ������ ü�� ���� ���� ��� �ð�

    public float CurrentHealth { get; private set; } // ���� ü�� (�ܺ� ���ٸ� ���)


    // ü�� (1 ~ 100 ���� ���� ���)
    [Range(1, 100)][SerializeField] private int health = 10;
    // �ܺο��� ���� ������ ������Ƽ (�� ���� �� �ڵ����� 0~100 ���̷� ����)
    public int Health
    {
        get => health;
        set => health = Mathf.Clamp(value, 0, 100);
    }

    // �̵� �ӵ� (1f ~ 20f ���� ���� ���)
    [Range(1f, 20f)][SerializeField] private float speed = 3;
    // �ܺο��� ���� ������ ������Ƽ (�� ���� �� 0~20f�� ����)
    public float Speed
    {
        get => speed;
        set => speed = Mathf.Clamp(value, 0, 20);
    }


    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        animationHandler = GetComponent<AnimationHandler>();

        // �������� �����Ǿ� �ִٸ� �����ؼ� ���� ��ġ�� ����
        if (WeaponPrefab != null)
            weaponHandler = Instantiate(WeaponPrefab, weaponPivot);
        else
            weaponHandler = GetComponentInChildren<WeaponHandler>(); // �̹� �پ� �ִ� ���� ���
    }

    protected virtual void Start()
    {
        CurrentHealth = Health;
    }

    protected virtual void Update()
    {
        HandleAction();

        // �ڵ� ���� Ȯ�ο�
        if(isAttacking == true)
        {
            Debug.Log("isAttacking true");
        }

        Rotate(lookDirection);
        HandleAttackDelay(); // ���� �Է� �� ��Ÿ�� ����

        // ���� ���� ���¶�� �ð� ����
        if (timeSinceLastChange < healthChangeDelay)
        {
            // ���� �ð� ���� �� �ִϸ��̼ǿ��� �˸�
            timeSinceLastChange += Time.deltaTime;
            if (timeSinceLastChange >= healthChangeDelay)
            {
                animationHandler.InvincibilityEnd();
            }
        }
    }

    protected virtual void FixedUpdate()
    {
        Movment(movementDirection);
        if (knockbackDuration > 0.0f)
        {
            knockbackDuration -= Time.fixedDeltaTime; // �˹� �ð� ����
        }
    }

    // ĳ����, �� �ڵ�׼� �������̵� �ޱ�
    protected virtual void HandleAction()
    {

    }


    // ĳ���� �̵�
    private void Movment(Vector2 direction)
    {
        direction = direction * Speed; // �̵� �ӵ�

        // �˹� ���̸� �̵� �ӵ� ���� + �˹� ���� ����
        if (knockbackDuration > 0.0f)
        {
            direction *= 0.2f; // �̵� �ӵ� ����
            direction += knockback; // �˹� ���� �߰�
        }

        // ���� ���� �̵�
        _rigidbody.velocity = direction;

        // �̵� �ִϸ��̼� ó��
        animationHandler.Move(direction);
    }

    // ��������Ʈ �¿� ����(ĳ��,����)
    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bool isLeft = Mathf.Abs(rotZ) > 90f;

        // ��������Ʈ �¿� ����
        characterRenderer.flipX = isLeft;

        if (weaponPivot != null)
        {
            // ���� ȸ�� ó��
            weaponPivot.rotation = Quaternion.Euler(0, 0, rotZ);
        }

        // ���⵵ �Բ� �¿� ���� ó��
        weaponHandler?.Rotate(isLeft);
    }

    // �˹�
    public void ApplyKnockback(Transform other, float power, float duration)
    {
        knockbackDuration = duration;
        // ��� ������ �ݴ�� �о
        knockback = -(other.position - transform.position).normalized * power;
    }

    // ü�� ���� �Լ� (���� or ȸ��)
    public bool ChangeHealth(float change)
    {
        // ��ȭ ���ų� ���� ���¸� ����
        if (change == 0 || timeSinceLastChange < healthChangeDelay)
        {
            return false;
        }

        timeSinceLastChange = 0f; // �ٽ� ���� ����

        // ü�� ����
        CurrentHealth += change;
        CurrentHealth = CurrentHealth > Health ? Health : CurrentHealth;
        CurrentHealth = CurrentHealth < 0 ? 0 : CurrentHealth;

        // �������� ��� (����)
        if (change < 0)
        {
            animationHandler.Damage(); // �´� �ִϸ��̼� ����

        }

        // ü���� 0 ���ϰ� �Ǹ� ��� ó��
        if (CurrentHealth <= 0f)
        {
            Death();
        }

        return true;
    }

    public virtual void Death()
    {
        // ������ ����
        _rigidbody.velocity = Vector3.zero;

        // ��� SpriteRenderer�� ���� ���缭 ���� ȿ�� ����
        foreach (SpriteRenderer renderer in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            Color color = renderer.color;
            color.a = 0.3f;
            renderer.color = color;
        }

        // ��� ������Ʈ(��ũ��Ʈ ����) ��Ȱ��ȭ
        foreach (Behaviour component in transform.GetComponentsInChildren<Behaviour>())
        {
            component.enabled = false;
        }

        // 2�� �� ������Ʈ �ı�
        Destroy(gameObject, 2f);
    }

    private void HandleAttackDelay()
    {
        if (weaponHandler == null)
            return;

        // ���� ��ٿ� ���̸� �ð� ����
        if (timeSinceLastAttack <= weaponHandler.Delay)
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        // ���� �Է� ���̰� ��Ÿ���� �������� ���� ����
        if (isAttacking && timeSinceLastAttack > weaponHandler.Delay)
        {
            timeSinceLastAttack = 0;
            Debug.Log("player is attacking");
            Attack(); // ���� ���� ����
        }
    }

    protected virtual void Attack()
    {
        // �ٶ󺸴� ������ ���� ���� ����
        if (lookDirection != Vector2.zero)
            weaponHandler?.Attack();
    }

}
