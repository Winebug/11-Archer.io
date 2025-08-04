using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecroMancer : Enemy
{
    [SerializeField] private BulletShooter bulletShooter;
    [SerializeField] private float fireCooldown = 2f;//총알 날라가는 쿨타임
    private float fireTimer = 2f;
    private Animator animator;
    private static readonly int IsInside = Animator.StringToHash("IsInside");
    [SerializeField] private GameObject[] summonPrefabs; //소환할 몬스터 프리팹들
    [SerializeField] private float summonCooldown = 10f; //소환 주기
    private float summonTimer = 9f;//소환 쿨타임

    [SerializeField] private GameObject magicCirclePrefab;

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

        summonTimer += Time.deltaTime;

        if (summonTimer >= summonCooldown)
        {
            SummonRandomMonster();
            summonTimer = 0f;
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
    private IEnumerator SummonWithMagicCircle(GameObject monsterPrefab, Vector2 spawnPos)
    {
        GameObject magic = Instantiate(magicCirclePrefab, spawnPos, Quaternion.identity);

        // 1초 대기 (연출 시간)
        yield return new WaitForSeconds(1f);

        // 몬스터 생성
        GameObject summonedMonster = Instantiate(monsterPrefab, spawnPos, Quaternion.identity);

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
}
