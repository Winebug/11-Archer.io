using UnityEngine;

[CreateAssetMenu(fileName = "MultiShotSkill", menuName = "Skill/MultiShotSkill")]
public class MultiShotSkill : Skill
{
    public override void Apply(Player player)
    {
        var weapon = player.GetComponentInChildren<WeaponHandler>();

        if (weapon == null)
        {
            Debug.Log("장착 중인 무기 없음.");
            return;
        }
        
        int currentStack = SkillManager.Instance.GetSkillStack(this);

        if (currentStack >= 3)
        {
            Debug.Log("멀티샷 최대 중첩 도달 (3회)");
            return;
        }

        player.MultiShotCount++;
        weapon.Delay *= 1.15f;
        player.MultiShotDamageMultiplier *= 0.90f;

        Debug.Log($"멀티샷 적용됨: 현재 중첩 {player.MultiShotCount}, 공격속도 감소, 공격력 감소");
    }
}
