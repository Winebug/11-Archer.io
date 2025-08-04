using UnityEngine;

[CreateAssetMenu(fileName = "DeadlyShot", menuName = "Skill/DeadlyShot")]
public class DeadlyShotSkill : Skill
{
    public override void Apply(Player player)
    {
        player.HasDeadlyShotEffect = true;
        Debug.Log("Deadly Shot 활성화됨.");
    }
}
