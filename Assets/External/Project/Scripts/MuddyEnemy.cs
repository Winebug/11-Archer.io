using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuddyEnemy : Enemy
{
    bool active = true;
    bool isHiding = false;

    Collider2D selfCollider;

    protected override void Awake()
    {
        base.Awake();

        selfCollider = GetComponentInChildren<Collider2D>();
    }

    protected override void Start()
    {
        base.Start();

        MoleMovemet();
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
        
    }


    IEnumerator MoleMovemet()
    {
        while (active)
        {
            //플레이어 근처로 순간이동 
            Teleport();

            //2초 후 공격
            yield return new WaitForSeconds(2f);
            isAttacking = true;

            //3초 후 공격
            yield return new WaitForSeconds(3f);
            HideAndSeek();

            yield return new WaitForSeconds(3f);
        }

    }

    void Teleport()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector2 target = (Vector2)playerTemp.position + Random.insideUnitCircle * 3f;

            //벽과 장애물에 순간이동 안되게 하는 코드, obstacleLayer에 벽과 장애물 설정 필요

            //if (!Physics2D.OverlapCircle(target, 0.5f, obstacleLayer))
            //{
                transform.position = target;
                return;
            //}

        }


    }

    void HideAndSeek()
    {
        if (isHiding)
        {
            selfCollider.enabled = true;
            characterRenderer.enabled = true;
        }
        else
        {
            selfCollider.enabled = false;
            characterRenderer.enabled = false;
        }
    }
}
