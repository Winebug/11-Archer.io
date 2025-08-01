using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskedOrcEnemy : Enemy
{
    Rigidbody2D rb;
    bool active = true;
    public bool isCharging = false;

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
        base.Update();
    }

    protected override void FixedUpdate()
    {
        if (isCharging)
        {
            Charging();
        }
    }

    protected override void HandleAction()
    {
        Vector2 direction = FaceDirection();
        lookDirection = direction;
        movementDirection = direction;
    }

    IEnumerator BoarMovemet()
    {

        while (active)
        {

            //3초간 정지

            rb.velocity = Vector3.zero;
            yield return new WaitForSeconds(3f);

            //돌진 준비 
            ReadyCharge();

            //2초 후 빠른 속도로 돌진
            yield return new WaitForSeconds(2f);
            isCharging = true;
            isAttacking = true;

            //충돌까지 코루틴 대기
            yield return new WaitWhile(() => isCharging);
            //충돌하면 3초 동안 어질어질
            Spinning();
            yield return new WaitForSeconds(2f);


        }

    }

    void ReadyCharge()
    {
        rb.velocity = -lookDirection;
    }

    void Charging()
    {
        Vector2 direction = lookDirection * 8f;
        rb.AddForce(direction);
    }

    void Spinning()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isCharging)
        {
            Recoll(collision.transform);
        }

    }

    public void Recoll(Transform target)
    {
        rb.velocity = Vector3.zero;
        isCharging = false;

        isAttacking = false;
        Vector2 backDirection = (this.transform.position - target.position).normalized * 3.0f;
        rb.AddForce(backDirection, ForceMode2D.Impulse);

    }

}
