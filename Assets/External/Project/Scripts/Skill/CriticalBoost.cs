using UnityEngine;

[CreateAssetMenu(fileName = "CriticalBoost", menuName = "Skill/CriticalBoost")]
public class CriticalBoostSkill : Skill
{
    public override void Apply(Player player)
    {
        var weapon = player.GetComponentInChildren<WeaponHandler>();
        if (weapon == null) return;

        Debug.Log("크리티컬 증가.");
    }
}
