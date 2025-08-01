using UnityEngine;

[CreateAssetMenu(fileName = "HealOnKill", menuName = "Skill/HealOnKill")]
public class HealOnKillSkill : Skill
{
    public override void Apply(Player player)
    {
        player.HasHealOnKillEffect = true;
        Debug.Log("Heal on Kill 스킬 적용됨. 적 처치 시 체력 회복.");
    }
}
