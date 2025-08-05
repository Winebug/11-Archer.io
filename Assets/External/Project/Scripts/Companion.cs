using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Companion : UnitController
{
    [SerializeField] float attakRange = 1;
    [SerializeField] float searchRange = 1;
    [SerializeField] float followRange = 1;

    //강의에서는 Init로 플레이어 할당해주므로, 아마 수정 예정
    protected Transform targetTemp;
    protected Transform followTarget;

    GameManager gameManager;

    public void Init(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }


    protected override void Start()
    {
        base.Start();

        GameObject playerObject = GameObject.FindWithTag("Player");
        followTarget = playerObject.transform;
    }

    protected override void HandleAction()
    {
        // 부모에서 정의한 기본 동작 처리
        //base.HandleAction();

        // 일정 주기마다 가장 가까운 적을 찾아서 targetTemp에 할당
        FindNearestEnemy();

        //isAttacking = false;

        float distance = DistanceBetween();
        Vector2 direction = FaceDirection();

        float distance2 = DistanceBetween2();
        Vector2 direction2 = FaceDirection2();

        // 타겟이 없다 and FollowTarget이 searchRange 범위내 or 플레이어랑 followRange 이상 떨어졌을경우
        if (targetTemp == null && distance2 <= searchRange || distance2 > followRange)
        {
            lookDirection = direction2;

            // follow target에게 접근
            movementDirection = direction2;

            // followTarget과 거리가 몇 이하이면 정지
            if (distance2 < 2f)
                movementDirection = Vector2.zero;

            if (targetTemp != null)
                lookDirection = direction;
            return;
        }

        Debug.Log("targetTemp" + targetTemp);

        Debug.Log("distance" + distance);

        // 타겟이 있고 searchRange 안에 있을 때만 추적 시작
        if (targetTemp != null && distance <= searchRange)
        {
            Debug.Log("타겟 발견" + targetTemp);

            lookDirection = direction;

            // follow target에게 접근
            movementDirection = direction;

            //적이 사거리에 들어오면, 공격
            if (distance <= weaponHandler.AttackRange)
            {
                int layerMaskTarget = weaponHandler.target;

                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, weaponHandler.AttackRange * 1.5f,
                    (1 << LayerMask.NameToLayer("Level")) | layerMaskTarget);

                // 레이어 마스크가 일치하는지 확인 후 공격
                if (hit.collider != null && layerMaskTarget == (layerMaskTarget | (1 << hit.collider.gameObject.layer)))
                {
                    isAttacking = true;
                }
                lookDirection = direction;
                movementDirection = Vector2.zero;
                return;
            }

        }
    }

    // 적과의 거리
    float DistanceBetween()
    {
        if (targetTemp != null)
            return Vector3.Distance(this.transform.position, targetTemp.position);

        return 0;
    }

    // 플레이어와의 거리
    float DistanceBetween2()
    {
        return Vector3.Distance(this.transform.position, followTarget.position);
    }

    // 적 방향
    protected Vector2 FaceDirection()
    {
        if (targetTemp != null)
            return (targetTemp.position - this.transform.position).normalized;

        return Vector2.zero;
    }

    // 플레이어 방향
    protected Vector2 FaceDirection2()
    {
        return (followTarget.position - this.transform.position).normalized;
    }

    // 공격범위,follow범위 확인용(오브젝트 선택시 나타남, 별도 함수호출X)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attakRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, searchRange);
    }

    //가장 가까운 적을 찾아 targetTemp에 할당하는 새로운 메서드 추가
    void FindNearestEnemy()
    {
        // "Enemy" 태그를 가진 모든 게임 오브젝트를 찾음
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        Transform nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        // 찾은 적들 중에서 가장 가까운 적을 선택
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);

            // 현재 찾은 적이 searchRange 안에 있고, 가장 가까운 적이면 업데이트
            if (distanceToEnemy <= searchRange && distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy.transform;
            }
        }

        // 가장 가까운 적이 있다면 targetTemp에 할당
        targetTemp = nearestEnemy;
    }
}
