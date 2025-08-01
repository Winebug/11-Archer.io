using UnityEngine;

[CreateAssetMenu(fileName = "WallReflection", menuName = "Skill/WallReflection")]
public class WallReflection : Skill
{
    public override void Apply(Player player) // 플레이어에 벽 반사 효과 적용
    {
        player.HasWallReflection = true; // 벽 반사 활성화 플래그 설정
        player.WallReflectionDamageMultiplier = 0.5f; // 튕김 시 데미지 80% 유지
        Debug.Log("벽 반사 스킬 활성화됨"); // 적용 로그 출력
    }
}