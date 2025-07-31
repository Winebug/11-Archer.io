using UnityEngine;

[CreateAssetMenu(fileName = "RicochetShot", menuName = "Skill/RicochetShot")]
public class RicochetShotSkill : Skill
{
    public override void Apply(Player player)
    {
        player.HasRicochetSkill = true;
        // 플레이어에 이 스킬 설정값도 저장하고 싶다면 Player에 필드 추가
        player.RicochetMaxBounces = 2;
        player.RicochetDamageMultiplier = 0.7f;
        Debug.Log("반동샷 활성화됨.");
    }
}
