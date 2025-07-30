using UnityEngine;

[CreateAssetMenu(fileName = "AttackDamageBoost", menuName = "Skill/AttackDamageBoost")]
public class AttackDamageBoostSkill : Skill
{
    public override void Apply(Player player)
    {
        var weapon = player.GetComponentInChildren<WeaponHandler>(); // 플레이어가 장착 중인 무기

        if (weapon == null) return;
        {
            Debug.Log("장착 중인 무기 없음.");
        }

        weapon.Power += 5f; // 증가하는 공격력
        Debug.Log("공격력 증가");
    }
}
