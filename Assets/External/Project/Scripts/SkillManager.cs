using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }
    [SerializeField] private List<Skill> skills = new List<Skill>(); // 스킬 리스트

    // 싱글톤 구현
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    public void UseSkill(int skillNum, UnitController player)
    {
        if (skillNum < 0 || skillNum >= skills.Count) // 스킬 불러오기 예외처리
        {
            Debug.Log("스킬 불러오기 실패.");
            return;
        }

        Skill skill = skills[skillNum];  // 스킬 리스트의 인덱스 지정

        if (skill != null && skill.canUseSkill) // 스킬 쿨타임이 아니라면 스킬 발동
        {
            skill.Activate(player);
            Debug.Log($"{skill.name} 발동.");
        }

        else
            Debug.Log($"{skill.name} 쿨타임중.");
    }
}
