using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillSelectorUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject panel;                // 전체 스킬 선택 패널
    [SerializeField] private Button[] skillButtons;           // 선택할 버튼 (3개)
    [SerializeField] private List<Skill> allSkills;           // 선택 풀

    private List<Skill> currentChoices = new List<Skill>();   // 현재 선택된 3개
    private Player player;                                    // 스킬을 적용할 대상 (플레이어)

    public void Initialize(Player playerRef)
    {
        player = playerRef;
    }

    public void Show()
    {
        if (skillButtons == null || skillButtons.Length < 3)
        {
            Debug.LogError("Skill Buttons 배열이 제대로 설정되지 않았습니다.");
            return;
        }

        if (panel == null)
        {
            Debug.LogError("Skill Selector Panel이 설정되지 않았습니다.");
            return;
        }

        panel.SetActive(true);
        Time.timeScale = 0f;

        currentChoices = GetRandomSkills(3);

        for (int i = 0; i < skillButtons.Length; i++)
        {
            int index = i;
            Skill skill = currentChoices[index];
            Image image = skillButtons[index].transform.Find("Icon")?.GetComponent<Image>();
            
            if (image != null && skill.icon != null)
            {
                image.sprite = skill.icon;
                image.preserveAspect = true;
                image.enabled = true;
            }
            else
            {
                Debug.LogWarning($"[SkillSelectorUI] Button {index}에 Icon 이미지가 없거나, skill.icon이 null입니다.");
            }

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

        foreach (Skill s in allSkills)
        {
            if (!SkillManager.Instance.IsSkillReachedAtMaxStack(s))
            {
                pool.Add(s);
            }
        }

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
