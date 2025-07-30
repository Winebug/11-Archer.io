using UnityEngine;

[CreateAssetMenu(fileName = "AttackSpeedBoost", menuName = "Skill/AttackSpeedBoost")]
public class AttackSpeedBoostSkill : Skill
{
    public override void Apply(Player player)
    {
        var weapon = player.GetComponentInChildren<WeaponHandler>();
        if (weapon == null) return;

        Debug.Log("공격 속도 증가.");
    }
}
