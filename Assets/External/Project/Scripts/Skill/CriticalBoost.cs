using UnityEngine;

[CreateAssetMenu(fileName = "CriticalBoost", menuName = "Skill/CriticalBoost")]
public class CriticalBoostSkill : Skill
{
    public override void Apply(Player player)
    {
        player.Critical += 10;

        Debug.Log("크리티컬 증가.");
    }
}
