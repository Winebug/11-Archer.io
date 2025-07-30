using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponHandler : WeaponHandler
{
    Collider2D weaponCollider;
    List<Collider2D> alredyHitTargets;


    protected override void Start()
    {
        base.Start();
        weaponCollider = GetComponentInChildren<Collider2D>();
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
        }

        alredyHitTargets = new List<Collider2D>();
    }

    public override void Attack()
    {
        base.Attack();
    }

    public override void Rotate(bool isLeft)
    {
        if (isLeft)
            transform.eulerAngles = new Vector3(0, 180, 0);
        else
            transform.eulerAngles = new Vector3(0, 0, 0);
    }

    
    //애니메이션에서 공격판정 시작 부분에 이 함수를 호출
    public void TurnOnCollider()
    {
        alredyHitTargets.Clear();
        weaponCollider.enabled = true;
    }

    public void meleeProcess(Collider2D meleeHitTarget)
    {
        if (((1 << meleeHitTarget.gameObject.layer) & target) != 0 && !alredyHitTargets.Contains(meleeHitTarget))
        {
            Debug.Log("공격 성공");


            // 데미지 계산

            alredyHitTargets.Add(meleeHitTarget);
        }
    }

    //애니메이션에서 공격판정 끝나는 부분에 이 함수를 호출
    public void TurnOffCollider()
    {
        weaponCollider.enabled = false;
    }
}
