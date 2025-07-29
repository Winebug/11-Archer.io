using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : UnitController
{
    [SerializeField] float attakRange = 1;
    
    
    //강의에서는 Init로 플레이어 할당해주므로, 아마 수정 예정
    [SerializeField] Transform playerTemp;


    protected override void HandleAction() 
    {

        // 타겟(플레이어)가 죽으면 움직이지 않음
        if (playerTemp == null)
        {
            
            return;
        }

        float distance = DistanceBetween();
        Vector2 direction = FaceDirection();

        isAttacking = false;    
        if (distance < attakRange)  //플레이어가 사거리에 들어오면, 공격
        {
            Debug.Log("공격 로직");
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
