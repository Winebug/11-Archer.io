using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; } // 싱글톤

    private List<Skill> skills = new List<Skill>(); // 현재 플레이어가 획득한 스킬 목록
    
    // 싱글톤
    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);

        else Instance = this;
    }

    // 스킬 선택
    public void AcquireSkill(Skill skill, Player player)
    {
        skills.Add(skill);
        skill.Apply(player);
    }
}
