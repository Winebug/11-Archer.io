using UnityEngine;

public class Skill : ScriptableObject
{
    public string skillName;
    public string description;
    public Sprite icon;

    public enum SkillType { Passive, Active }
    public SkillType skillType;
    public bool canStack;
    public bool cannotStackMoreThanOne;
    public int maxSkillStacks = 0;

    // 스킬 효과 반영
    public virtual void Apply(Player player)
    {
        Debug.Log($"[Skill] {skillName} applied");
        WeaponHandler weapon = player.GetComponentInChildren<WeaponHandler>();
    }

    public virtual bool CanShow() // 스킬 ui 에 표시해도 되는가
    {
        return true;
    }
}
