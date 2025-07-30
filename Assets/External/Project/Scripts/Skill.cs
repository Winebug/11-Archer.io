using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    [SerializeField] protected float cooldown = 5f; // 스킬 쿨타임
    protected float lastUsedTime = -Mathf.Infinity; // 스킬을 마지막으로 사용한 시점
    public bool canUseSkill { get { return Time.time >= lastUsedTime + cooldown; } } // 스킬 사용 여부 판단
    public abstract void Activate(UnitController user); // 스킬 활성화

}
