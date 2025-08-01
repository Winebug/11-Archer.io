using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieEnemy : Enemy
{
    private Animator animator;
    private static readonly int IsInside = Animator.StringToHash("IsInside");
    protected override void Start()
    {
        base.Start();
        animator = GetComponentInChildren<Animator>();

    }

    protected override void Update()
    {
        base.Update();

        if (animator != null && animator.GetBool(IsInside))
        {
            movementDirection = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            animator?.SetBool(IsInside, true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            animator?.SetBool(IsInside, false);
        }
    }
}
