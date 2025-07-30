using UnityEngine;

// ëª¨ë“  ìºë¦­?„°?˜ ê¸°ë³¸ ???ì§ì„, ?šŒ? „, ?„‰ë°? ì²˜ë¦¬ë¥? ?‹´?‹¹?•˜?Š” ê¸°ë°˜ ?´?˜?Š¤
public class UnitController : MonoBehaviour
{
    protected Rigidbody2D _rigidbody; // ?´?™?„ ?œ„?•œ ë¬¼ë¦¬ ì»´í¬?„Œ?Š¸

    protected AnimationHandler animationHandler;

    [SerializeField] private SpriteRenderer characterRenderer; // ì¢Œìš° ë°˜ì „?„ ?œ„?•œ ? Œ?”?Ÿ¬
    [SerializeField] private Transform weaponPivot; // ë¬´ê¸°ë¥? ?šŒ? „?‹œ?‚¬ ê¸°ì?? ?œ„ì¹?

    [SerializeField] protected float healthChangeDelay = .5f; // ?”¼?•´ ?›„ ë¬´ì  ì§??† ?‹œê°?


    protected Vector2 movementDirection = Vector2.zero; // ?˜„?¬ ?´?™ ë°©í–¥
    public Vector2 MovementDirection { get { return movementDirection; } }

    protected Vector2 lookDirection = Vector2.zero; // ?˜„?¬ ë°”ë¼ë³´ëŠ” ë°©í–¥
    public Vector2 LookDirection { get { return lookDirection; } }

    private Vector2 knockback = Vector2.zero; // ?„‰ë°? ë°©í–¥
    private float knockbackDuration = 0.0f; // ?„‰ë°? ì§??† ?‹œê°?

    [SerializeField] public WeaponHandler WeaponPrefab; // ?¥ì°©í•  ë¬´ê¸° ?”„ë¦¬íŒ¹ (?—†?œ¼ë©? ??‹?—?„œ ì°¾ì•„ ?‚¬?š©)
    protected WeaponHandler weaponHandler; // ?¥ì°©ëœ ë¬´ê¸°

    protected bool isAttacking; // °ø°İ Áß ¿©ºÎ
    protected float attackRange; // °ø°İ ¹üÀ§
    private float timeSinceLastAttack = float.MaxValue; // ¸¶Áö¸· °ø°İ ÀÌÈÄ °æ°ú ½Ã°£

    protected float timeSinceLastChange = float.MaxValue; // ë§ˆì??ë§? ì²´ë ¥ ë³?ê²? ?´?›„ ê²½ê³¼ ?‹œê°?

    public float CurrentHealth { get; private set; } // ?˜„?¬ ì²´ë ¥ (?™¸ë¶? ? ‘ê·¼ë§Œ ?—ˆ?š©)


    // ì²´ë ¥ (1 ~ 100 ?‚¬?´ ê°’ë§Œ ?—ˆ?š©)
    [Range(1, 100)][SerializeField] private int health = 10;
    // ?™¸ë¶??—?„œ ? ‘ê·? ê°??Š¥?•œ ?”„ë¡œí¼?‹° (ê°? ë³?ê²? ?‹œ ??™?œ¼ë¡? 0~100 ?‚¬?´ë¡? ? œ?•œ)
    public int Health
    {
        get => health;
        set => health = Mathf.Clamp(value, 0, 100);
    }

    // ?´?™ ?†?„ (1f ~ 20f ?‚¬?´ ê°’ë§Œ ?—ˆ?š©)
    [Range(1f, 20f)][SerializeField] private float speed = 3;
    // ?™¸ë¶??—?„œ ? ‘ê·? ê°??Š¥?•œ ?”„ë¡œí¼?‹° (ê°? ë³?ê²? ?‹œ 0~20fë¡? ? œ?•œ)
    public float Speed
    {
        get => speed;
        set => speed = Mathf.Clamp(value, 0, 20);
    }


    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        animationHandler = GetComponent<AnimationHandler>();

        // ?”„ë¦¬íŒ¹?´ ì§?? •?˜?–´ ?ˆ?‹¤ë©? ?ƒ?„±?•´?„œ ?¥ì°? ?œ„ì¹˜ì— ë¶?ì°?
        if (WeaponPrefab != null)
            weaponHandler = Instantiate(WeaponPrefab, weaponPivot);
        else
            weaponHandler = GetComponentInChildren<WeaponHandler>(); // ?´ë¯? ë¶™ì–´ ?ˆ?Š” ë¬´ê¸° ?‚¬?š©
    }

    protected virtual void Start()
    {
        CurrentHealth = Health;
    }

    protected virtual void Update()
    {
        HandleAction();

        // ??™ ê³µê²© ?™•?¸?š©
        if(isAttacking == true)
        {
            Debug.Log("isAttacking true");
        }

        if (movementDirection.magnitude > 0)
        {
            Rotate(lookDirection);
        }
        HandleAttackDelay(); // ê³µê²© ?…? ¥ ë°? ì¿¨í???„ ê´?ë¦?

        
    }

    protected virtual void FixedUpdate()
    {
        Movment(movementDirection);
        if (knockbackDuration > 0.0f)
        {
            knockbackDuration -= Time.fixedDeltaTime; // ?„‰ë°? ?‹œê°? ê°ì†Œ
        }
    }

    // ìºë¦­?„°, ?  ?•¸?“¤?•¡?…˜ ?˜¤ë²„ë¼?´?“œ ë°›ê¸°
    protected virtual void HandleAction()
    {

    }


    // ìºë¦­?„° ?´?™
    private void Movment(Vector2 direction)
    {
        direction = direction * Speed; // ?´?™ ?†?„

        // ?„‰ë°? ì¤‘ì´ë©? ?´?™ ?†?„ ê°ì†Œ + ?„‰ë°? ë°©í–¥ ? ?š©
        if (knockbackDuration > 0.0f)
        {
            direction *= 0.2f; // ?´?™ ?†?„ ê°ì†Œ
            direction += knockback; // ?„‰ë°? ë°©í–¥ ì¶”ê??
        }

        // ?‹¤? œ ë¬¼ë¦¬ ?´?™
        _rigidbody.velocity = direction;

        // ?´?™ ?• ?‹ˆë©”ì´?…˜ ì²˜ë¦¬
        if (animationHandler != null)
            animationHandler.Move(direction);
    }

    // ?Š¤?”„?¼?´?Š¸ ì¢Œìš° ë°˜ì „(ìºë¦­,ë¬´ê¸°)
    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bool isLeft = Mathf.Abs(rotZ) > 90f;

        // ?Š¤?”„?¼?´?Š¸ ì¢Œìš° ë°˜ì „
        characterRenderer.flipX = isLeft;

        if (weaponPivot != null)
        {
            // ë¬´ê¸° ?šŒ? „ ì²˜ë¦¬
            weaponPivot.rotation = Quaternion.Euler(0, 0, rotZ);
        }

        // ë¬´ê¸°?„ ?•¨ê»? ì¢Œìš° ë°˜ì „ ì²˜ë¦¬
        weaponHandler?.Rotate(isLeft);
    }

    // ?„‰ë°?
    public void ApplyKnockback(Transform other, float power, float duration)
    {
        knockbackDuration = duration;
        // ?ƒ??? ë°©í–¥?„ ë°˜ë??ë¡? ë°??–´?ƒ„
        knockback = -(other.position - transform.position).normalized * power;
    }

    // ì²´ë ¥ ë³?ê²? ?•¨?ˆ˜ (?”¼?•´ or ?šŒë³?)
    public bool ChangeHealth(float change)
    {
        // ë³??™” ?—†ê±°ë‚˜ ë¬´ì  ?ƒ?ƒœë©? ë¬´ì‹œ
        if (change == 0 || timeSinceLastChange < healthChangeDelay)
        {
            return false;
        }

        timeSinceLastChange = 0f; // ?‹¤?‹œ ë¬´ì  ?‹œ?‘

        // ì²´ë ¥ ? ?š©
        CurrentHealth += change;
        CurrentHealth = CurrentHealth > Health ? Health : CurrentHealth;
        CurrentHealth = CurrentHealth < 0 ? 0 : CurrentHealth;

        // ?°ë¯¸ì???¼ ê²½ìš° (?Œ?ˆ˜)
        if (change < 0)
        {
            animationHandler.Damage(); // ë§ëŠ” ?• ?‹ˆë©”ì´?…˜ ?‹¤?–‰

        }

        // ì²´ë ¥?´ 0 ?´?•˜ê°? ?˜ë©? ?‚¬ë§? ì²˜ë¦¬
        if (CurrentHealth <= 0f)
        {
            Death();
        }

        return true;
    }

    public virtual void Death()
    {
        // ???ì§ì„ ? •ì§?
        _rigidbody.velocity = Vector3.zero;

        // ëª¨ë“  SpriteRenderer?˜ ?ˆ¬ëª…ë„ ?‚®ì¶°ì„œ ì£½ì?? ?š¨ê³? ?—°ì¶?
        foreach (SpriteRenderer renderer in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            Color color = renderer.color;
            color.a = 0.3f;
            renderer.color = color;
        }

        // ëª¨ë“  ì»´í¬?„Œ?Š¸(?Š¤?¬ë¦½íŠ¸ ?¬?•¨) ë¹„í™œ?„±?™”
        foreach (Behaviour component in transform.GetComponentsInChildren<Behaviour>())
        {
            component.enabled = false;
        }

        // 2ì´? ?›„ ?˜¤ë¸Œì ?Š¸ ?ŒŒê´?
        Destroy(gameObject, 2f);
    }

    private void HandleAttackDelay()
    {
        if (weaponHandler == null)
            return;

        // ê³µê²© ì¿¨ë‹¤?š´ ì¤‘ì´ë©? ?‹œê°? ?ˆ„? 
        if (timeSinceLastAttack <= weaponHandler.Delay)
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        // ê³µê²© ?…? ¥ ì¤‘ì´ê³? ì¿¨í???„?´ ??‚¬?œ¼ë©? ê³µê²© ?‹¤?–‰
        if (isAttacking && timeSinceLastAttack > weaponHandler.Delay)
        {
            timeSinceLastAttack = 0;
            Debug.Log("player is attacking");
            Attack(); // ½ÇÁ¦ °ø°İ ½ÇÇà
        }
    }

    protected virtual void Attack()
    {
        // ë°”ë¼ë³´ëŠ” ë°©í–¥?´ ?ˆ?„ ?•Œë§? ê³µê²©
        if (lookDirection != Vector2.zero)
            weaponHandler?.Attack();
    }

}
