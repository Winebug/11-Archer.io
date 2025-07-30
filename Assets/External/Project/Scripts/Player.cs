using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : UnitController
{
    [SerializeField] private LayerMask enemyLayer; // 공격 대상 레이어
    private SkillManager sm;
    protected override void Start()
    {
        base.Start();
        sm = SkillManager.Instance; // 스킬매니저 싱글톤을 sm 변수에 저장
    }


    protected override void Update()
    {
        base.Update();
        // ���� ���� ���¶�� �ð� ����
        if (timeSinceLastChange < healthChangeDelay)
        {
            // ���� �ð� ���� �� �ִϸ��̼ǿ��� �˸�
            timeSinceLastChange += Time.deltaTime;
            if (timeSinceLastChange >= healthChangeDelay)
            {
                animationHandler.InvincibilityEnd();
            }
        }

        if (Input.GetKeyDown(KeyCode.J) && sm != null) // J 키 누르면 0 번 스킬 발동
        {
            sm.UseSkill(0, this); // 0 번 스킬 발동
        }
        
        if (Input.GetKeyDown(KeyCode.K) && sm != null) // K 키 누르면 1 번 스킬 발동
        {
            sm.UseSkill(1, this); // 1 번 스킬 발동
        }
    }

    protected override void HandleAction()
    {
        // ????? ????? ???? ??? ???? ???? (??/??/??/??)
        float horizontal = Input.GetAxisRaw("Horizontal"); // A/D ??? ??/??
        float vertical = Input.GetAxisRaw("Vertical"); // W/S ??? ??/??

        // bool hDown = Input.GetButtonDown("Horizontal");
        // bool vDown = Input.GetButtonDown("Vertical");
        // bool hUp = Input.GetButtonDown("Horizontal");
        // bool vUp = Input.GetButtonDown("Vertical");

        // // ???? ???? ????? (?�O???? ?? ??? ????)
        movementDirection = new Vector2(horizontal, vertical).normalized; // 벡터 정규화

        // lookDirection = new Vector2(horizontal, 0);

        // ???? ???? ?? ????
        isAttacking = true; // 상시 공격 상태

        if (movementDirection.magnitude > 0.01f) // 이동 중인지 확인
        {
            lookDirection = movementDirection; // 바라보는 방향을 이동방향과 같게 설정
            isAttacking = false; // 자동 공격 중지
            return;
        }

        Transform closestTarget = FindClosestTarget(); // 가장 가까운 적 탐색 메서드의 리턴값을 closestTarget 변수에 저장

        if (closestTarget != null)
        {
            Vector2 target = (closestTarget.position - this.transform.position).normalized; // 타겟 벡터값 설정
            lookDirection = target; // 발견된 타겟을 바라보게 설정
            isAttacking = true; // 타겟 공격
        }

        else isAttacking = false;
    }

    // 가장 가까운 적 탐색
    private Transform FindClosestTarget()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer); // 공격 범위 내에서 적 검출
        Transform closest = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider2D target in targets) // 검출된 타겟들 정보 루프화
        {
            float distance = Vector2.Distance(transform.position, target.transform.position); 

            if (distance < minDistance) // 가까운 적 설정 조건문
            {
                minDistance = distance;
                closest = target.transform;
            }
        }

        return closest; // 최종적으로 가장 가까운 적 위치를 반환
    }

    public override void Death()
    {
        base.Death();
        Debug.Log("Game Over.");
        //gameManager.GameOver(); // ???? ???? ??? 
    }

    protected override void ShowAttackRange()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}