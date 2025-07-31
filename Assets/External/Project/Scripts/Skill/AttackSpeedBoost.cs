using UnityEngine;

[CreateAssetMenu(fileName = "AttackSpeedBoost", menuName = "Skill/AttackSpeedBoost")]
public class AttackSpeedBoostSkill : Skill
{
    public override void Apply(Player player)
    {
        var weapon = player.GetComponentInChildren<WeaponHandler>();

        if (weapon == null)
        {
            Debug.Log("장착중인 무기 없음.");
            return;
        }

        weapon.Delay -= Mathf.Max(0.1f, weapon.Delay - weapon.Delay * 0.25f); // 최소 딜레이 = 0.1
        Debug.Log("공격 속도 증가.");
    }
}
