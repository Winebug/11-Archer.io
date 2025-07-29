using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float attakRange = 1;
    
    
    //���ǿ����� Init�� �÷��̾� �Ҵ����ֹǷ�, �Ƹ� ���� ����
    [SerializeField] Transform playerTemp;

    Rigidbody2D rb;//�׽�Ʈ��. ���� ���ӿ����� ��� X
    private void Start()//�׽�Ʈ��. ���� ���ӿ����� ��� X
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() //�׽�Ʈ��. ���� ���ӿ����� ��� X
    {
        ChangeThisNameLater();
    }

    void ChangeThisNameLater() //�θ� Ŭ���� ��ӹ޾ƿͼ� �̸� ����
    {

        // Ÿ��(�÷��̾�)�� ������ �������� ����
        if (playerTemp == null)
        {
            
            return;
        }

        float distance = DistanceBetween();
        Vector2 direction = FaceDirection();

        //�÷��̾ �ٶ󺸰� �ϱ�


        if (distance < attakRange)//�÷��̾ ��Ÿ��� ������, ����
        {
            Debug.Log("���� ����");
        }

        // �÷��̾�� ����
        // �̰͵� �θ� Ŭ������ ���� ��
        rb.velocity = direction;



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
