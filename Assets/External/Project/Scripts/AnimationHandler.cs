using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    // Animator 파라미터 이름을 미리 해시로 변환해 캐싱 (성능 최적화)
    private static readonly int IsMoving = Animator.StringToHash("IsMove");
    private static readonly int IsDamage = Animator.StringToHash("IsDamage");

    protected Animator animator;

    protected virtual void Awake()
    {
        // 애니메이터 컴포넌트를 자식에서 가져옴
        //animator = GetComponentInChildren<Animator>();
        animator = GetComponent<Animator>();
    
        if (animator == null)
            animator = GetComponentInChildren<Animator>(); // 자식까지 검색
        
        if (animator == null)
            Debug.LogWarning("Animator not found on " + gameObject.name);
    }

    public void Move(Vector2 obj)
    {
        // 이동 방향 벡터의 크기를 이용해 움직이는 중인지 판단
        animator.SetBool(IsMoving, obj.magnitude > .5f);
    }

    public void Damage()
    {
        // 피격 애니메이션 상태 진입
        animator.SetBool(IsDamage, true);
    }

    public void InvincibilityEnd()
    {
        // 무적 시간 종료 시 호출
        animator.SetBool(IsDamage, false);
    }

    public void ForcedStopMove()
    {
        animator.SetBool(IsMoving, false);
    }
}