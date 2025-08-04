using UnityEngine;

[CreateAssetMenu(fileName = "HpBoost", menuName = "Skill/HpBoost")]
public class HpBoostSkill : Skill
{
    public override void Apply(Player player)
    {
        int originalMaxHp = player.Health;
        int bonusHp = Mathf.FloorToInt(originalMaxHp * 0.2f);

        player.Health += bonusHp;
        player.ChangeHealth(bonusHp);

        Debug.Log($"최대 체력 {originalMaxHp} => {player.Health}, 체력 {bonusHp} 회복됨");
    }
}
