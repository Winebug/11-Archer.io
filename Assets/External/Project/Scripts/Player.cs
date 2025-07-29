using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : UnitController
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void HandleAction()
    {
        // ????? ????? ???? ??? ???? ???? (??/??/??/??)
        float horizontal = Input.GetAxisRaw("Horizontal"); // A/D ??? ??/??
        float vertical = Input.GetAxisRaw("Vertical"); // W/S ??? ??/??

        bool hDown = Input.GetButtonDown("Horizontal");
        bool vDown = Input.GetButtonDown("Vertical");
        bool hUp = Input.GetButtonDown("Horizontal");
        bool vUp = Input.GetButtonDown("Vertical");

        // ???? ???? ????? (?“O???? ?? ??? ????)
        movementDirection = new Vector2(horizontal, vertical).normalized;

        lookDirection = new Vector2(horizontal, 0);

        // ???? ???? ?? ????
        isAttacking = true;

    }

    public override void Death()
    {
        base.Death();
        //gameManager.GameOver(); // ???? ???? ??? 
    }
}