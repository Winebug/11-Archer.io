using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuddyEnemy : Enemy
{
    bool active = true;
    bool isHiding = true;

    Collider2D selfCollider;

    protected override void Awake()
    {
        base.Awake();

        selfCollider = GetComponentInChildren<Collider2D>();
    }

    protected override void Start()
    {
        base.Start();

        StartCoroutine(MoleMovemet());

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
            HideAndSeek();

            //2초 후 공격
            yield return new WaitForSeconds(2f);
            isAttacking = true;

            //3초 후 숨기
            yield return new WaitForSeconds(3f);
            isAttacking = false;
            HideAndSeek();

            yield return new WaitForSeconds(3f);

        }

    }

    void Teleport()
    {
        if (playerTemp == null)
            return;
        
        for (int i = 0; i < 10; i++)
        {
            Vector2 target = RandomDonutPosition(playerTemp.position, 3f, 5f);

            i++;

            //벽과 장애물에 순간이동 안되게 하는 코드, obstacleLayer에 벽과 장애물 설정 필요

            //if (!Physics2D.OverlapCircle(target, 0.5f, obstacleLayer))
            //{
                transform.position = target;
                return;
            //}

            
        }


    }

    Vector2 RandomDonutPosition(Vector2 center, float minRadius, float maxRadius)
    {
        float angle = Random.Range(0f, 2f * Mathf.PI);
        float radius = Random.Range(minRadius, maxRadius); 

        Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
        return center + offset;
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
        isHiding = !isHiding;
    }
}
