using UnityEngine;

[CreateAssetMenu(fileName = "Invincibility", menuName = "Skill/Invincibility")]
public class Invincibility : Skill
{
    public override void Apply(Player player)
    {
        player.EnableInvincibility();
        Debug.Log("자동 무적 스킬 적용됨: 10초마다 2초간 무적 상태.");
    }
}
