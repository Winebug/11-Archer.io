using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : UnitController
{
    [SerializeField] float attakRange = 1;
    
    
    //강의에서는 Init로 플레이어 할당해주므로, 아마 수정 예정
    [SerializeField] Transform playerTemp;
    [SerializeField] private MonsterStat statData;
    protected override void Start()
    {
        base.Start();

        if (statData != null) //몬스터 기본스탯등을 불러와서 초기화
        {
            Health = statData.health;
            Speed = statData.moveSpeed;
            Health = statData.health;
            weaponHandler = statData.weaponPrefab;
        }
    }
    protected override void HandleAction()
    {

        // 타겟(플레이어)가 없으면 움직이지 않음
        if (playerTemp == null)
        {

            return;
        }

        float distance = DistanceBetween();
        Vector2 direction = FaceDirection();

        isAttacking = false;

        lookDirection = direction;

        //플레이어가 사거리에 들어오면, 공격

        if (distance < attakRange)
        {
            int layerMaskTarget = weaponHandler.target;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, weaponHandler.AttackRange * 1.5f,
                (1 << LayerMask.NameToLayer("Level")) | layerMaskTarget);

            if (hit.collider != null && layerMaskTarget == (layerMaskTarget | (1 << hit.collider.gameObject.layer)))
            {
                isAttacking = true;
            }

            movementDirection = Vector2.zero;
            return;
        }

        // 플레이어에게 접근
        movementDirection = direction;

    }

    float DistanceBetween()
    {
        return Vector3.Distance(this.transform.position, playerTemp.position);
    }

    Vector2 FaceDirection()
    {
        return (playerTemp.position - this.transform.position).normalized;
    }
}
