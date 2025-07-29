using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerController : UnitController
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void HandleAction()
    {
        // Ű���� �Է��� ���� �̵� ���� ��� (��/��/��/��)
        float horizontal = Input.GetAxisRaw("Horizontal"); // A/D �Ǵ� ��/��
        float vertical = Input.GetAxisRaw("Vertical"); // W/S �Ǵ� ��/��

        bool hDown = Input.GetButtonDown("Horizontal");
        bool vDown = Input.GetButtonDown("Vertical");
        bool hUp = Input.GetButtonDown("Horizontal");
        bool vUp = Input.GetButtonDown("Vertical");

        // ���� ���� ����ȭ (�밢���� �� �ӵ� ����)
        movementDirection = new Vector2(horizontal, vertical).normalized;

        lookDirection = new Vector2(horizontal,0);

        isAttacking = true;
    }
}