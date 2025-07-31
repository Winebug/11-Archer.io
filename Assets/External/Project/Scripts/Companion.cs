using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Companion : UnitController
{
    [SerializeField] float attakRange = 1;
    [SerializeField] float followRange = 1;

    //강의에서는 Init로 플레이어 할당해주므로, 아마 수정 예정
    [SerializeField] protected Transform targetTemp;
    [SerializeField] protected Transform followTarget;
    [SerializeField] private CompanionStat statData;
    public CompanionStat StatData => statData;
    protected override void Start()
    {
        base.Start();

        if (statData != null) // 기본스탯등을 불러와서 초기화
        {
            Health = statData.health;
            Speed = statData.moveSpeed;
            Health = statData.health;
            weaponHandler = statData.weaponPrefab;
        }
    }

    protected override void HandleAction()
    {
        // 부모에서 정의한 기본 동작 처리
        base.HandleAction();

        isAttacking = false;

        float distance2 = DistanceBetween2();
        Vector2 direction2 = FaceDirection2();

        // 타겟이 없다 and FollowTarget이 FollowRange 범위내 or 플레이어랑 3이상 떨어졌을경우
        if (targetTemp == null && distance2 <= followRange || distance2 > 3)
        {
            lookDirection = direction2;

            // follow target에게 접근
            movementDirection = direction2;

            // followTarget과 거리가 몇 이하이면 정지
            if (distance2 < 2f)
                movementDirection = Vector2.zero;
            return;
        }

        // 타겟이 있고 FollowRange 안에 있을 때만 추적 시작
        if (targetTemp != null)
        {
            
            float distance = DistanceBetween();
            Vector2 direction = FaceDirection();

            lookDirection = direction;

            // follow target에게 접근
            movementDirection = direction;

            if (distance <= followRange)
            {
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
                        Debug.Log($"{gameObject.name} {isAttacking}");
                    }
                    lookDirection = direction;
                    movementDirection = Vector2.zero;
                    return;
                }
            }
        }
    }

    // 적과의 거리
    float DistanceBetween()
    {
        return Vector3.Distance(this.transform.position, targetTemp.position);
    }

    // 플레이어와의 거리
    float DistanceBetween2()
    {
        return Vector3.Distance(this.transform.position, followTarget.position);
    }

    // 적 방향
    protected Vector2 FaceDirection()
    {
        return (targetTemp.position - this.transform.position).normalized;
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
        Gizmos.DrawWireSphere(transform.position, followRange);
    }
}
