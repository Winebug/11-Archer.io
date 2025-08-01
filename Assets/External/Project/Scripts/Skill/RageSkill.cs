using UnityEngine;

[CreateAssetMenu(fileName = "RageSkill", menuName = "Skill/RageSkill")]
public class RageSkill : Skill
{
    public override void Apply(Player player)
    {
        player.IsRage = true;
        Debug.Log("Rage 스킬 적용: 체력이 낮을수록 공격력이 증가합니다.");
    }
}
