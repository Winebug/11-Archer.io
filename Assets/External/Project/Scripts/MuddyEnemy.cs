using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuddyEnemy : Enemy
{
    //IndexOutOfRangeException: Index was outside the bounds of the array.
    //이 경고가 뜨면 ProjectileManager의 ProjectilePrefab에다가 Projectile MudBall 추가해주세요 

    public Vector2 roomOrigin;
    Vector2 roomHalfSize = new Vector2(2f, 4f);

    bool active = true;
    bool isHiding = true;
    private static readonly int IsHiding = Animator.StringToHash("IsHiding");

    Collider2D col;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] GameObject hpBar;

    protected override void Awake()
    {
        base.Awake();
        col = GetComponent<Collider2D>();
    }

    protected override void Start()
    {
        base.Start();
        lookDirection = Vector3.up;
        roomOrigin = transform.root.position;
        if (roomOrigin == Vector2.zero)
            Debug.LogWarning("Muddy가 방의 좌표를 불러오지 못했습니다");
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
            yield return new WaitForSeconds(3f);

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

            if (!Physics2D.OverlapCircle(target, 0.5f, obstacleLayer) && IsInsideRoom(target))
            {
                transform.position = target;
                return;
            }

        }

        Debug.LogWarning("순간이동 실패");
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
            hpBar.SetActive(true);
        }
        else
        {
            col.enabled = false;
            characterRenderer.enabled = false;
            hpBar.SetActive(false);
        }
        isHiding = !isHiding;
    }

    bool IsInsideRoom(Vector2 pos)
    {
        Vector2 minRoomPos = roomOrigin - roomHalfSize;
        Vector2 maxRoomPos = roomOrigin + roomHalfSize;

        return pos.x >= minRoomPos.x && pos.x <= maxRoomPos.x &&
               pos.y >= minRoomPos.y && pos.y <= maxRoomPos.y;
    }
}
