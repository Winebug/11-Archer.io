using UnityEngine;

public class SlugEnemy : Enemy
{
    [SerializeField] private BulletShooter bulletShooter;
    [SerializeField] private float fireCooldown = 2f;//총알 날라가는 쿨타임
    private float fireTimer = 2f;
    private static readonly int IsInside = Animator.StringToHash("IsInside");
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private LayerMask playerLayer;
    private bool isPlayerInside = false;

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
        bool playerDetected = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);
        if (playerDetected)
        {
            if (isPlayerInside)
            {
                animator?.SetBool(IsInside, true);
                isPlayerInside = false;
            }

            if (fireTimer >= fireCooldown)
            {
                bulletShooter?.ShootBullet();
                fireTimer = 0f;
            }
        }
        else
        {
            if (isPlayerInside)
            {
                animator?.SetBool(IsInside, false);
                isPlayerInside = false;
            }
        }

        fireTimer += Time.deltaTime;
    }

}
