using UnityEngine;

[CreateAssetMenu(fileName = "AddProjectile", menuName = "Skill/AddProjectile")]
public class AddProjectileSkill : Skill
{
    public override void Apply(Player player)
    {
        var weapon = player.GetComponentInChildren<WeaponHandler>();

        if (weapon == null) return;
        {
            Debug.Log("장착 중인 무기 없음.");
        }

        weapon.AddProjectiles();
        Debug.Log("발사체 추가.");
    }
}
