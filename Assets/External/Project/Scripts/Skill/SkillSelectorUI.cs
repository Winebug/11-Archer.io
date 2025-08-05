using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelectorUI : MonoBehaviour
{
    public static SkillSelectorUI Instance;

    [Header("UI Elements")]
    [SerializeField] private GameObject panel;
    [SerializeField] private Button[] skillButtons;
    [SerializeField] private List<Skill> allSkills;

    [Header("Effect")]
    [SerializeField] private ParticleSystem clickEffect;  // 클릭 이펙트

    private List<Skill> currentChoices = new List<Skill>();
    private Player player;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    public void Initialize(Player playerRef)
    {
        player = playerRef;
    }

    public void Show()
    {
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

            skillButtons[index].onClick.RemoveAllListeners();
            skillButtons[index].onClick.AddListener(() => OnSkillSelected(skill, skillButtons[index]));
        }
    }

    private void OnSkillSelected(Skill skill, Button clickedButton)
    {
        // 파티클 이펙트 재생
        if (clickEffect != null)
        {
            clickEffect.transform.position = clickedButton.transform.position;
            clickEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            clickEffect.Play();
        }

        SkillManager.Instance.AcquireSkill(skill, player);

        panel.SetActive(false);
        Time.timeScale = 1f;
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
