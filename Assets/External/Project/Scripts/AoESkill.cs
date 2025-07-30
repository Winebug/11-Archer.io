using UnityEngine;

public class AoESkill : Skill
{
    [SerializeField] private float skillRadius = 3f; // 스킬 범위
    [SerializeField] private int damage = 5; // 스킬 데미지
    [SerializeField] private LayerMask targetLayer; // 공격할 대상 레이어 설정

    // 스킬 발동
    public override void Activate(UnitController player)
    {
        if (!canUseSkill) return; // 스킬이 쿨타임일 경우 스킬 발동 방지

        lastUsedTime = Time.time; // 마지막으로 스킬을 사용한 시간 기록

        Collider2D[] targets = Physics2D.OverlapCircleAll(player.transform.position, skillRadius, targetLayer); // 플레이어 위치 기준으로 공격 대상 추적

        foreach (Collider2D target in targets)
        {
            UnitController enemy = target.GetComponent<UnitController>(); // 대상 확인

            if (enemy != null)
                enemy.ChangeHealth(-damage); // 확인된 대상에게 스킬 데미지 적용
        }

        Debug.Log("AoE 스킬 사용됨: " + targets.Length + "명 적중");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, skillRadius);
    }
}
