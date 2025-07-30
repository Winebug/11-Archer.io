using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Skill/NewSkill")]
public class Skill : ScriptableObject
{
    public string skillName;
    public string description;
    public Sprite icon;

    public enum SkillType { Passive, Active }
    public SkillType skillType;

    // 스킬 효과 반영
    public virtual void Apply(Player player)
    {
        Debug.Log($"[Skill] {skillName} applied");
        WeaponHandler weapon = player.GetComponentInChildren<WeaponHandler>();
    }
}
