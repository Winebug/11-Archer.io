using UnityEngine;

[CreateAssetMenu(fileName = "AddProjectile", menuName = "Skill/AddProjectile")]
public class AddProjectileSkill : Skill
{
    public override void Apply(Player player)
    {
        var weapon = player.GetComponentInChildren<RangeWeaponHandler>();

        if (weapon == null) 
        {
            Debug.Log("RangeWeaponHandler 없음.");
            return;
        }

        weapon.AddProjectiles();
        Debug.Log("발사체 추가.");
    }
}
