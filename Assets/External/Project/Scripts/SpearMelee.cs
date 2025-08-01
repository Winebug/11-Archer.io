using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearMelee : MeleeWeaponHandler
{
    MaskedOrcEnemy maskedOrc;
    private static readonly int IsCharging = Animator.StringToHash("IsCharging");


    protected override void Start()
    {
        base.Start();
        maskedOrc = GetComponentInParent<MaskedOrcEnemy>();

    }

    private void Update()
    {
        if (maskedOrc.isCharging)
        {
            animator.SetBool(IsCharging, true);
        }
    }

    public override void meleeProcess(Collider2D meleeHitTarget)
    {
        if (meleeHitTarget.gameObject == maskedOrc.gameObject)
            return;
        
        base.meleeProcess(meleeHitTarget);

        maskedOrc.Recoll(meleeHitTarget.transform);
        animator.SetBool(IsCharging, false);
    }
}
