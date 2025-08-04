using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecroMancer : Enemy
{
    [SerializeField] private BulletShooter bulletShooter;
    [SerializeField] private float fireCooldown = 2f;//총알 날라가는 쿨타임
    private float fireTimer = 2f;
    private static readonly int IsInside = Animator.StringToHash("IsInside");
    [SerializeField] private GameObject[] summonPrefabs; //소환할 몬스터 프리팹들
    [SerializeField] private float summonCooldown = 10f; //소환 주기
    private float summonTimer = 9f;//소환 쿨타임

    [SerializeField] private GameObject magicCirclePrefab;
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
        summonTimer += Time.deltaTime;

        if (summonTimer >= summonCooldown)
        {
            SummonRandomMonster();
            summonTimer = 0f;
        }
    }


    private IEnumerator SummonWithMagicCircle(GameObject monsterPrefab, Vector2 spawnPos)
    {
        GameObject magic = Instantiate(magicCirclePrefab, spawnPos, Quaternion.identity);

        // 1초 대기 (연출 시간)
        yield return new WaitForSeconds(1f);

        // 몬스터 생성
        Instantiate(monsterPrefab, spawnPos, Quaternion.identity);

        // 마법진은 2초 후 자동 제거
        Destroy(magic, 2f);
    }

    private void SummonRandomMonster()
    {
        if (summonPrefabs.Length == 0) return;

        int index = Random.Range(0, summonPrefabs.Length);
        Vector2 spawnPos = (Vector2)transform.position + Random.insideUnitCircle * 5f;

        StartCoroutine(SummonWithMagicCircle(summonPrefabs[index], spawnPos));
    }
    public override void Death()
    {
        base.Death();

        if (GameManager.instance != null)
        {
            GameManager.instance.StageClear();
        }
        else
        {
            Debug.LogWarning("GameManager 인스턴스를 찾을 수 없습니다.");
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
#endif
}
