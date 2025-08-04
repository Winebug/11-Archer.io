using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchWeaponHandler : WeaponHandler
{
    Collider2D weaponCollider;
    List<Collider2D> alredyHitTargets;
    [SerializeField] bool shouldRotate = false;


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
        if (shouldRotate)
        {
            return;
        }

        if (isLeft)
            transform.eulerAngles = new Vector3(0, 180, 0);
        else
            transform.eulerAngles = new Vector3(0, 0, 0);
    }

    
    public void TurnOnCollider()
    {
        alredyHitTargets.Clear();
        weaponCollider.enabled = true;
    }

    public void TurnOffCollider()
    {
        weaponCollider.enabled = false;
    }

    public virtual void PunchProcess(Collider2D target)
    {
        if (((1 << target.gameObject.layer) & this.target) != 0 && !alredyHitTargets.Contains(target))
        {
            Debug.Log("첫if문");
            UnitController unit = target.GetComponent<UnitController>();
            if (unit != null)
            {
                unit.ChangeHealth(-Power);
                Debug.Log("공격 성공");

                if (IsOnKnockback)
                    unit.ApplyKnockback(transform, KnockbackPower, KnockbackTime);
            }

            alredyHitTargets.Add(target);
        }
    }

}
