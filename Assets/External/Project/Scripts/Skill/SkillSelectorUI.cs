using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillSelectorUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject panel;                // 전체 스킬 선택 패널
    [SerializeField] private Button[] skillButtons;           // 선택할 버튼 (3개)
    [SerializeField] private List<Skill> allSkills;           // 선택 풀 (에디터에서 등록)

    private List<Skill> currentChoices = new List<Skill>();   // 현재 선택된 3개
    private Player player;                                    // 스킬을 적용할 대상

    public void Initialize(Player playerRef)
    {
        player = playerRef;
    }

    public void Show()
    {
        panel.SetActive(true);
        Time.timeScale = 0f; // 게임 일시정지

        currentChoices = GetRandomSkills(3);

        for (int i = 0; i < skillButtons.Length; i++)
        {
            int index = i;
            Skill skill = currentChoices[index];

            skillButtons[index].GetComponentInChildren<Text>().text = skill.skillName;

            skillButtons[index].onClick.RemoveAllListeners();
            skillButtons[index].onClick.AddListener(() => OnSkillSelected(skill));
        }
    }

    private void OnSkillSelected(Skill skill)
    {
        SkillManager.Instance.AcquireSkill(skill, player);

        panel.SetActive(false);
        Time.timeScale = 1f; // 게임 재개
    }

    private List<Skill> GetRandomSkills(int count)
    {
        List<Skill> pool = new List<Skill>(allSkills);
        List<Skill> result = new List<Skill>();

        for (int i = 0; i < count && pool.Count > 0; i++)
        {
            int rand = Random.Range(0, pool.Count);
            result.Add(pool[rand]);
            pool.RemoveAt(rand);
        }

        return result;
    }
}
