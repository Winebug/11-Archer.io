using UnityEngine;

public class SlugEnemy : Enemy
{
    [SerializeField] private BulletShooter bulletShooter;
    [SerializeField] private float fireCooldown = 2f;//총알 날라가는 쿨타임
    private float fireTimer = 2f;
    private static readonly int IsInside = Animator.StringToHash("IsInside");

    protected override void Start()
    {
        base.Start();
        animator = GetComponentInChildren<Animator>();

        if (bulletShooter == null)
        {
            bulletShooter = GetComponentInChildren<BulletShooter>();
        }
    }

    protected override void Update()
    {
        base.Update();

        fireTimer += Time.deltaTime;

        if (animator != null && animator.GetBool(IsInside))
        {
            movementDirection = Vector2.zero;

            if (fireTimer >= fireCooldown)
            {
                bulletShooter?.ShootBullet();
                fireTimer = 0f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            animator?.SetBool(IsInside, true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            animator?.SetBool(IsInside, false);
        }
    }
}
