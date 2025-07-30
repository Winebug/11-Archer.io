using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Player : UnitController
{
    [SerializeField] private LayerMask enemyLayer; // 공격 대상 레이어
    private WeaponHandler wh;
    private int critical = 0;
    public int Critical
    {
        get => critical;
        set => critical = Math.Clamp(value, 0, 100);
    }
    private SkillManager sm;
    protected override void Start()
    {
        base.Start();
        // sm = SkillManager.Instance; // 스킬매니저 싱글톤을 sm 변수에 저장
        ShowSkillSelectorUI();
        wh = GetComponentInChildren<WeaponHandler>();
    }

    [SerializeField] private SkillSelectorUI skillSelectorUI;


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
    }

    protected override void HandleAction()
    {
        float horizontal = Input.GetAxisRaw("Horizontal"); 
        float vertical = Input.GetAxisRaw("Vertical"); 

        movementDirection = new Vector2(horizontal, vertical).normalized; // 벡터 정규화

        isAttacking = true; // 상시 공격 상태

        if (movementDirection.magnitude > 0.01f) // 이동 중인지 확인
        {
            lookDirection = movementDirection; // 바라보는 방향을 이동방향과 같게 설정
            isAttacking = false; // 자동 공격 중지
            return;
        }

        Transform closestTarget = FindClosestTarget(); // 가장 가까운 적 탐색 메서드의 리턴값을 closestTarget 변수에 저장

        if(closestTarget == null) // 만약 탐색된 적이 없다면
        {
            Debug.Log("closestTarget is null");
            isAttacking = false; // 공격 중지
            return; // 메서드 종료
        }

        else if (closestTarget != null)
        {
            Debug.Log("closestTarget found");
            Vector2 target = (closestTarget.position - this.transform.position).normalized; // 타겟 벡터값 설정
            lookDirection = target; // 발견된 타겟을 바라보게 설정
            isAttacking = true; // 타겟 공격
        }

        else isAttacking = false;
    }

    // 가장 가까운 적 탐색
    private Transform FindClosestTarget()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, wh.AttackRange, enemyLayer); // 공격 범위 내에서 적 검출
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
        Gizmos.DrawWireSphere(transform.position, wh.AttackRange);
    }

    // temp 메서드. 나중에 스킬 습득 조건 발동에 맞춰 변경 예정
    private void ShowSkillSelectorUI()
    {
        skillSelectorUI.Initialize(this);
        skillSelectorUI.Show();
    }

    // 디버깅용 기즈모는 OnDrawGizmosSelected()에서 호출
    // void OnDrawGizmosSelected() // 이 함수는 Unity가 자동으로 호출
    // {
    //     // 오브젝트가 선택되었을 때만 기즈모를 그리도록
    //     // attackRange가 0보다 큰지 확인하는 것이 좋습니다.
    //     if (showattackRange > 0f)
    //     {
    //         Gizmos.color = Color.red; // 기즈모 색상 설정
    //         Gizmos.DrawWireSphere(transform.position, showattackRange); // attackRange 원 그리기
    //     }
    // }
}