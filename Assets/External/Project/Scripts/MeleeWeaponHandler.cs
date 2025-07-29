using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponHandler : WeaponHandler
{
    [Header("Melee Attack Info")]
    public Vector2 collideBoxSize = Vector2.one;

    protected override void Start()
    {
        base.Start();
        collideBoxSize = collideBoxSize * WeaponSize;
    }

    public override void Attack()
    {
        base.Attack();

        RaycastHit2D hit = Physics2D.BoxCast(transform.position + (Vector3)Controller.LookDirection * collideBoxSize.x, collideBoxSize, 0, Vector2.zero, 0, target);

        if (hit.collider != null)
        {
            //충돌 대상의 체력을 깎을 수 있다면, 깎기
            
        }
    }

    public override void Rotate(bool isLeft)
    {
        if (isLeft)
            transform.eulerAngles = new Vector3(0, 180, 0);
        else
            transform.eulerAngles = new Vector3(0, 0, 0);
    }
}
