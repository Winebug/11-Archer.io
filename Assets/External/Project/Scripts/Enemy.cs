using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float attakRange = 1;
    
    
    //강의에서는 Init로 플레이어 할당해주므로, 아마 수정 예정
    [SerializeField] Transform playerTemp;

    Rigidbody2D rb;//테스트용. 실제 게임에서는 사용 X
    private void Start()//테스트용. 실제 게임에서는 사용 X
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() //테스트용. 실제 게임에서는 사용 X
    {
        ChangeThisNameLater();
    }

    void ChangeThisNameLater() //부모 클래스 상속받아와서 이름 수정
    {

        // 타겟(플레이어)가 죽으면 움직이지 않음
        if (playerTemp == null)
        {
            
            return;
        }

        float distance = DistanceBetween();
        Vector2 direction = FaceDirection();

        //플레이어를 바라보게 하기


        if (distance < attakRange)//플레이어가 사거리에 들어오면, 공격
        {
            Debug.Log("공격 로직");
        }

        // 플레이어에게 접근
        // 이것도 부모 클래스에 있을 듯
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
