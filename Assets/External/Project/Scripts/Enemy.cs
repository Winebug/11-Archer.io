using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : UnitController
{
    [SerializeField] float attakRange = 0.1f;

    [SerializeField] protected Transform playerTemp;
    [SerializeField] private MonsterStat statData;
    public MonsterStat StatData => statData;

    // 🔹 Enemy가 속한 Room 참조
    public Room room;

    protected override void Start()
    {
        base.Start();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTemp = playerObj.transform;
        }
        else
        {
            Debug.LogWarning($"{name}: Player 태그를 가진 오브젝트를 찾을 수 없습니다.");
        }

        if (statData != null)
        {
            ResetHealthStat(statData.health);
            Speed = statData.moveSpeed;
            if (weaponHandler == null)
                weaponHandler = statData.weaponPrefab;
            if (weaponHandler != null)
                weaponHandler.Power *= statData.attackPower;
        }
    }

    protected override void HandleAction()
    {
        if (playerTemp == null) return;

        float distance = DistanceBetween();
        Vector2 direction = FaceDirection();

        isAttacking = false;
        lookDirection = direction;

        // 공격 범위 체크
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

        // 플레이어 쫓기
        movementDirection = direction;
    }

    float DistanceBetween()
    {
        return Vector3.Distance(transform.position, playerTemp.position);
    }

    protected Vector2 FaceDirection()
    {
        return (playerTemp.position - transform.position).normalized;
    }
    public void SetRoom(Room r)
    {
        room = r;
    }

    public override void Death()
    {
        base.Death();
        Debug.Log($"{name} 죽음 → Room에 보고");

        if (room != null)
        {
            room.OnEnemyDeath(this);
        }
        else
        {
            Debug.LogWarning($"{name}의 Room 참조가 없음!");
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null && statData != null)
            {
                Debug.Log($"{statData.attackPower} 피해를 플레이어에게 줌");
                player.ChangeHealth(-statData.attackPower);
            }
        }
    }
}
