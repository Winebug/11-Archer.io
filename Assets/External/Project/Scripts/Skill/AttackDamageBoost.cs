using UnityEngine;

[CreateAssetMenu(fileName = "AttackDamageBoost", menuName = "Skill/AttackDamageBoost")]
public class AttackDamageBoostSkill : Skill
{
    public override void Apply(Player player)
    {
        var weapon = player.GetComponentInChildren<WeaponHandler>();
        if (weapon == null) return;

        Debug.Log("공격력 증가.");
    }
}
