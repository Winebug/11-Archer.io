using UnityEngine;

[CreateAssetMenu(fileName = "RicochetShot", menuName = "Skill/RicochetShot")]
public class RicochetShotSkill : Skill
{
    public override void Apply(Player player)
    {
        player.HasRicochetSkill = true;
        player.RicochetMaxBounces = 3;
        player.RicochetDamageMultiplier = 0.7f;
        Debug.Log("반동샷 활성화됨.");
    }
}
