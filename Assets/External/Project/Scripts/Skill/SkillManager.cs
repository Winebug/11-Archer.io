using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; } // 싱글톤
    private HashSet<Skill> skillHashSet = new(); // 한번만 적용 가능한 스킬들 목록
    private Dictionary<Skill, int> skillDictionary = new(); // 누적 스킬들 목록
    private List<Skill> skills = new List<Skill>(); // 현재 플레이어가 획득한 스킬 목록
    [SerializeField] private SkillSelectorUI skillSelectorUI;

    // 싱글톤
    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);

        else Instance = this;
    }

    // 스킬 선택
    public void AcquireSkill(Skill skill, Player player)
    {
        if (skill == null || player == null) return;

        skills.Add(skill);
        ApplySkill(player, skill);
    }

    public void ApplySkill(Player player, Skill skill)
    {
        if (skill.cannotStackMoreThanOne)
        {
            if (skillHashSet.Contains(skill)) return;

            skill.Apply(player);
            skillHashSet.Add(skill);
        }

        else if (skill.canStack)
        {
            skill.Apply(player);

            if (!skillDictionary.ContainsKey(skill))
                skillDictionary[skill] = 1;
            else skillDictionary[skill]++;
        }

        else skill.Apply(player);
    }

    public int GetSkillStack(Skill skill)
    {
        if (skillDictionary.TryGetValue(skill, out int stack))
            return stack;
        return 0;
    }

    public bool IsSkillReachedAtMaxStack(Skill skill)
    {
        if (skill == null) return true;

        if (skill.cannotStackMoreThanOne)
            return skillHashSet.Contains(skill);

        if (skill.canStack && skill.maxSkillStacks > 0)
            return GetSkillStack(skill) >= skill.maxSkillStacks;

        return false;
    }

    // 나중에 구현
    // private void OnEnable()
    // {
    //     FindObjectOfType<Player>().OnSkillSelectionTriggered += ShowSkillSelector;        
    // }

    // private void OnDisable()
    // {
    //     FindObjectOfType<Player>().OnSkillSelectionTriggered -= ShowSkillSelector;    
    // }
    // private void ShowSkillSelector(Player player)
    // {
    //     skillSelectorUI.Initialize(player);
    //     skillSelectorUI.Show();
    // }
}
