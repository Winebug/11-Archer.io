using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskedOrcEnemy : Enemy
{
    Rigidbody2D rb;
    bool active = true;
    bool isCharging = false;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void Start()
    {
        base.Start();

        StartCoroutine(BoarMovemet());

    }

    protected override void Update()
    {
        if (playerTemp == null)
        {
            active = false;
            return;
        }
    }

    protected override void FixedUpdate()
    {
        if (isCharging)
        {
            Charging();
        }
    }

    IEnumerator BoarMovemet()
    {

        while (active)
        {
            //돌진 준비 
            ReadyCharge();

            //5초 후 빠른 속도로 돌진
            yield return new WaitForSeconds(5f);
            isCharging = true;
            isAttacking = true;

            //충돌하면 3초 동안 어질어질
            Spinning();
            yield return new WaitForSeconds(3f);


        }

    }

    void ReadyCharge()
    {

    }

    void Charging()
    {
        Vector2 direction = FaceDirection() * 8f;

        rb.AddForce(direction);
    }

    void Spinning()
    {

    }

}
