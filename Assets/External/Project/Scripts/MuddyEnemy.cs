using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuddyEnemy : Enemy
{
    bool active = true;
    bool isHiding = true;
    Animator animator;
    private static readonly int IsHiding = Animator.StringToHash("IsHiding");

    Collider2D col;
    [SerializeField] LayerMask obstacleLayer;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponentInChildren<Animator>();
        col = GetComponent<Collider2D>();
    }

    protected override void Start()
    {
        base.Start();
        lookDirection = Vector3.up;
        StartCoroutine(MoleMovemet());

    }

    protected override void Update()
    {
        if (playerTransform == null)
        {
            active = false;
            return;
        }
        base.Update();
    }

    protected override void FixedUpdate()
    {
        
    }

    protected override void HandleAction()
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
            Debug.Log("isAttacking");

            //3초 후 숨기
            yield return new WaitForSeconds(2f);
            isAttacking = false;
            animator.SetBool(IsHiding, true);
            yield return new WaitForSeconds(1f);
            HideAndSeek();

            yield return new WaitForSeconds(3f);

        }

    }

    void Teleport()
    {
        if (playerTransform == null)
            return;
        
 

        for (int i = 0; i < 20; i++)
        {
            Vector2 target = RandomDonutPosition(playerTransform.position, 3f, 5f);
            i++;


            //벽과 장애물에 순간이동 안되게 하는 코드, obstacleLayer에 벽과 장애물 설정 필요

            if (!Physics2D.OverlapCircle(target, 0.5f, obstacleLayer))
            {
                transform.position = target;
                return;
            }



        }

        return;
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
            col.enabled = true;
            characterRenderer.enabled = true;
            animator.SetBool(IsHiding, false);
        }
        else
        {
            col.enabled = false;
            characterRenderer.enabled = false;
        }
        isHiding = !isHiding;
    }
}
