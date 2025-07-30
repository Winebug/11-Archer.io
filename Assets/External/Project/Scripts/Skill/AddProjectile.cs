using UnityEngine;

[CreateAssetMenu(fileName = "AddProjectile", menuName = "Skill/AddProjectile")]
public class AddProjectileSkill : Skill
{
    public override void Apply(Player player)
    {
        var weapon = player.GetComponentInChildren<RangeWeaponHandler>();

        if (weapon == null) return;

        Debug.Log("화살 추가.");
    }
}
