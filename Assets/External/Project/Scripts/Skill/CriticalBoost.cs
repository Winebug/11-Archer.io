using UnityEngine;

[CreateAssetMenu(fileName = "CriticalBoost", menuName = "Skill/CriticalBoost")]
public class CriticalBoostSkill : Skill
{
    public override void Apply(Player player)
    {
        player.Critical += 5;
        player.CriticalDamage += 0.2f;
        Debug.Log("크리티컬 증가.");
    }
}
