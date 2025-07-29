using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : UnitController
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        // 아직 무적 상태라면 시간 누적
        if (timeSinceLastChange < healthChangeDelay)
        {
            // 무적 시간 종료 시 애니메이션에도 알림
            timeSinceLastChange += Time.deltaTime;
            if (timeSinceLastChange >= healthChangeDelay)
            {
                animationHandler.InvincibilityEnd();
            }
        }
    }

    protected override void HandleAction()
    {
        // 키보드 입력을 통해 이동 방향 계산 (좌/우/상/하)
        float horizontal = Input.GetAxisRaw("Horizontal"); // A/D 또는 ←/→
        float vertical = Input.GetAxisRaw("Vertical"); // W/S 또는 ↑/↓

        bool hDown = Input.GetButtonDown("Horizontal");
        bool vDown = Input.GetButtonDown("Vertical");
        bool hUp = Input.GetButtonDown("Horizontal");
        bool vUp = Input.GetButtonDown("Vertical");

        // 방향 벡터 정규화 (대각선일 때 속도 보정)
        movementDirection = new Vector2(horizontal, vertical).normalized;

        lookDirection = new Vector2(horizontal, 0);

        // 상시 공격 중 상태
        isAttacking = true;

    }

    public override void Death()
    {
        base.Death();
        //gameManager.GameOver(); // 게임 오버 처리
    }
}