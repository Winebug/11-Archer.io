using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    private List<Skill> skills = new List<Skill>();

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        
        else Instance = this;
    }

    public void AcquireSkill(Skill skill, Player player)
    {
        if (skills.Contains(skill)) return;

        skills.Add(skill);
        skill.Apply(player);
    }

    public bool HasSkill(Skill skill) => skills.Contains(skill);
}
