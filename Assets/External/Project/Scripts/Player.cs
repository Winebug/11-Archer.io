using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : UnitController
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private SkillSelectorUI skillSelectorUI;
    protected override void Start()
    {
        base.Start();
        ShowSkillSelectorUI();
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
        movementDirection = new Vector2(horizontal, vertical).normalized;

        // lookDirection = new Vector2(horizontal, 0);

        // ???? ???? ?? ????
        isAttacking = true;

        if (movementDirection.magnitude > 0.01f)
        {
            lookDirection = movementDirection;
            isAttacking = false;
            return;
        }

        Transform closestTarget = FindClosestTarget();

        if (closestTarget != null)
        {
            Vector2 target = (closestTarget.position - this.transform.position).normalized;
            lookDirection = target;
            isAttacking = true;
        }

        else isAttacking = false;
    }

    private Transform FindClosestTarget()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);
        Transform closest = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider2D target in targets)
        {
            float distance = Vector2.Distance(transform.position, target.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                closest = target.transform;
            }
        }

        return closest;
    }

    public override void Death()
    {
        base.Death();
        //gameManager.GameOver(); // ???? ???? ??? 
    }

    private void ShowAttackRange()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    // temp 메서드. 나중에 스킬 습득 조건 발동에 맞춰 변경 예정
    private void ShowSkillSelectorUI()
    {
        skillSelectorUI.Initialize(this);
        skillSelectorUI.Show();
    }
}