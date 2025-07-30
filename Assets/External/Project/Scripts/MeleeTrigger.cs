using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeTrigger : MonoBehaviour
{
    

    MeleeWeaponHandler melee;

    private void Awake()
    {
        melee = GetComponentInParent<MeleeWeaponHandler>();
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        melee.meleeProcess(target);
    }

    public void TurnOnCollider()
    {
        melee.TurnOnCollider();
    }

    public void TurnOffCollider()
    {
        melee.TurnOffCollider();
    }
}
