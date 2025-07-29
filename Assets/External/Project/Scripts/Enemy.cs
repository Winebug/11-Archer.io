using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : UnitController
{
    [SerializeField] float attakRange = 1;
    
    
    //���ǿ����� Init�� �÷��̾� �Ҵ����ֹǷ�, �Ƹ� ���� ����
    [SerializeField] Transform playerTemp;


    protected override void HandleAction() 
    {

        // Ÿ��(�÷��̾�)�� ������ �������� ����
        if (playerTemp == null)
        {
            
            return;
        }

        float distance = DistanceBetween();
        Vector2 direction = FaceDirection();

        isAttacking = false;    
        if (distance < attakRange)  //�÷��̾ ��Ÿ��� ������, ����
        {
            Debug.Log("���� ����");
        }

        // �÷��̾�� ����
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
