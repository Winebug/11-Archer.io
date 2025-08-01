using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieEnemy : Enemy
{
    private Animator[] animators;
    private static readonly int IsInside = Animator.StringToHash("IsInside");
    private static readonly int IsAttack = Animator.StringToHash("IsAttack");
    protected override void Start()
    {
        base.Start();
        animators = GetComponentsInChildren<Animator>();

    }

    protected override void Update()
    {
        base.Update();

        if (animators[0] != null && animators[0].GetBool(IsInside))
        {
            movementDirection = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            animators[0]?.SetBool(IsInside, true);
            animators[1]?.SetBool(IsAttack, true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            animators[0]?.SetBool(IsInside, false);
            animators[1]?.SetBool(IsAttack, false);
        }
    }
}
