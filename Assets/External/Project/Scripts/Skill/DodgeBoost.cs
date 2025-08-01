using UnityEngine;

[CreateAssetMenu(fileName = "DodgeBoost", menuName = "Skill/DodgeBoost")]
public class DodgeBoost : Skill
{
    public override void Apply(Player player)
    {
        player.Dodge += 15;
        Debug.Log("회피 확률 증가.");
    }
}
