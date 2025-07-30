using UnityEngine;
using System.Collections;

public class MultiHitSkill : Skill
{
    [SerializeField] private float delay = 0.3f; // 연속공격 간격
    [SerializeField] private int multiHitCount = 3; // 공격 횟수
    [SerializeField] private int damage = 3; // 공격 데미지 (1 회당)
    [SerializeField] private float skillRange = 2f; // 공격 범위
    [SerializeField] private LayerMask targetLayer; // 공격할 대상 레이어 설정

    // 스킬 발동
    public override void Activate(UnitController player)
    {
        if (!canUseSkill) return;

        lastUsedTime = Time.time; // 마지막으로 스킬을 사용한 시간 기록
        player.StartCoroutine(ActivateMultiHit(player)); // 연속 공격 코루틴
    }

    // 연속공격 활성화
    private IEnumerator ActivateMultiHit(UnitController player)
    {
        for (int i = 0; i < multiHitCount; i++)
        {
            Collider2D[] targets = Physics2D.OverlapCircleAll(player.transform.position, skillRange, targetLayer); // 플레이어 위치 기준으로 공격 대상 추적

            foreach (Collider2D target in targets)
            {
                UnitController enemy = target.GetComponent<UnitController>(); // 대상 확인

                if (enemy != null)
                    enemy.ChangeHealth(-damage); // 확인된 대상에게 스킬 데미지 적용
            }

            yield return new WaitForSeconds(delay); // 연속 공격 사이에 delay 만큼 시간차를 설정
        }

        Debug.Log("연속공격 스킬 완료.");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, skillRange);
    }
}
