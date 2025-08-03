using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : UnitController
{
    [SerializeField] float attakRange = 0.1f;

    protected Transform playerTransform;
    [SerializeField] private MonsterStat statData;
    public MonsterStat StatData => statData;
    protected override void Start()
    {
        base.Start();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }

        if (playerTransform == null)
        {
            Debug.LogWarning($"{name}: Player 레이어를 가진 오브젝트를 찾을 수 없습니다.");
        }

        if (statData != null) //몬스터 기본스탯등을 불러와서 초기화
        {
            ResetHealthStat(statData.health);
            Speed = statData.moveSpeed;
            if (weaponHandler ==  null) 
                weaponHandler = statData.weaponPrefab;
            if (weaponHandler != null)
                weaponHandler.Power *= statData.attackPower;
        }
    }
    protected override void HandleAction()
    {

        // 타겟(플레이어)가 없으면 움직이지 않음
        if (playerTransform == null)
        {

            return;
        }

        float distance = DistanceBetween();
        Vector2 direction = FaceDirection();

        isAttacking = false;

        lookDirection = direction;

        //플레이어가 사거리에 들어오면, 공격

        if (distance < attakRange)
        {
            int layerMaskTarget = weaponHandler.target;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, weaponHandler.AttackRange * 1.5f,
                (1 << LayerMask.NameToLayer("Level")) | layerMaskTarget);

            if (hit.collider != null && layerMaskTarget == (layerMaskTarget | (1 << hit.collider.gameObject.layer)))
            {
                isAttacking = true;
            }

            movementDirection = Vector2.zero;
            return;
        }

        // 플레이어에게 접근
        movementDirection = direction;

    }

    float DistanceBetween()
    {
        return Vector3.Distance(this.transform.position, playerTransform.position);
    }

    protected Vector2 FaceDirection()
    {
        return (playerTransform.position - this.transform.position).normalized;
    }

    public override void Death()
    {
        Debug.Log("Enemy Death");
        base.Death();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null && statData != null)
            {
                Debug.Log(statData.attackPower + "피해를 줌");
                player.ChangeHealth(-statData.attackPower);
            }
        }
    }
}
