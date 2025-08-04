using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements.Experimental;

public class Player : UnitController
{
    private GameManager gameManager;
    private Camera playerCamera;

    [SerializeField] private LayerMask enemyLayer; // 공격 대상 레이어
    private WeaponHandler wh;
    private int dodge = 0;
    public int Dodge
    {
        get => dodge;
        set => dodge = Mathf.Clamp(value, 0, 45);
    }
    private int critical = 0;
    public int Critical
    {
        get => critical;
        set => critical = Math.Clamp(value, 0, 100);
    }

    private float criticalDamage = 1.0f;
    public float CriticalDamage
    {
        get => criticalDamage;
        set => criticalDamage = value;
    }
    public bool HasDeadlyShotEffect { get; set; } = false;
    public bool IsRage { get; set; } = false;
    public bool HasHealOnKillEffect { get; set; } = false;
    [SerializeField] private bool hasInvincibilitySkill = false;
    public bool HasInvincibilitySkill => hasInvincibilitySkill;
    private float invincibleCooldown = 10f;
    private float invincibleDuration = 2f;
    private float invincibleTimer = 0f;
    private bool isInvincible = false;
    public bool HasRicochetSkill { get; set; } = false;
    public int RicochetMaxBounces { get; set; } = 0;
    public float RicochetDamageMultiplier { get; set; } = 1f;
    public int MultiShotCount { get; set; } = 0;
    public float MultiShotDamageMultiplier { get; set; } = 1f;
    public bool HasWallReflection { get; set; } = false;
    public float WallReflectionDamageMultiplier { get; set; } = 1f;
    //public delegate void SkillSelectionEvent(Player player);
    //public event SkillSelectionEvent OnSkillSelectionTriggered;

    private SkillManager sm;

    //플레이어 키 설정
    private KeyBindingManager keyBindingManager;

    protected override void Start()
    {
        base.Start();
        keyBindingManager = FindObjectOfType<KeyBindingManager>();
        // sm = SkillManager.Instance; // 스킬매니저 싱글톤을 sm 변수에 저장
        // if (skillSelectorUI != null)
        //     ShowSkillSelectorUI();
        wh = GetComponentInChildren<WeaponHandler>();
        Debug.Log(CurrentHealth);
        if (skillSelectorUI != null)
            skillSelectorUI.Initialize(this);
        else
            if ("Tutorial" != SceneManager.GetActiveScene().name)
            Debug.LogWarning("스킬을 불러올 수 없습니다");
    }

    [SerializeField] private SkillSelectorUI skillSelectorUI;


    protected override void Update()
    {
        base.Update();
        // 무적 스킬 발동
        if (hasInvincibilitySkill)
        {
            invincibleTimer += Time.deltaTime;

            if (!isInvincible && invincibleTimer >= invincibleCooldown)
            {
                invincibleTimer = 0f;
                StartCoroutine(ApplyInvincibility());
            }
        }
        // ���� ���� ���¶�� �ð� ����
        if (timeSinceLastChange < healthChangeDelay)
        {
            // ���� �ð� ���� �� �ִϸ��̼ǿ��� �˸�
            timeSinceLastChange += Time.deltaTime;
            if (timeSinceLastChange >= healthChangeDelay)
            {
                animationHandler.InvincibilityEnd();
            }
        }
    }

    public void Init(GameManager gameManager)
    {
        this.gameManager = gameManager; // 게임 매니저 연결
        playerCamera = Camera.main; // 메인 카메라 참조 획득
    }

    private IEnumerator ApplyInvincibility()
    {
        isInvincible = true;
        timeSinceLastChange = 0f;
        // 무적 효과 시작 애니메이션 혹은 이펙트 추가
        yield return new WaitForSeconds(invincibleDuration);

        isInvincible = false;
        // 무적 효과 끝 애니메이션 혹은 이펙트 추가
    }

    protected override void HandleAction()
    {
        Vector2 direction = Vector2.zero;

        if (keyBindingManager != null)
        {
            direction = keyBindingManager.GetMovementInput();
        }
        else
        {
            Debug.Log("keyBindingManager가 없음");
        }

        movementDirection = direction.normalized;

        isAttacking = true; // 상시 공격 상태

        if (movementDirection.magnitude > 0.01f) // 이동 중인지 확인
        {
            lookDirection = movementDirection; // 바라보는 방향을 이동방향과 같게 설정
            isAttacking = false; // 자동 공격 중지
            return;
        }

        Transform closestTarget = FindClosestTarget(); // 가장 가까운 적 탐색 메서드의 리턴값을 closestTarget 변수에 저장

        if (closestTarget == null) // 만약 탐색된 적이 없다면
        {
            isAttacking = false; // 공격 중지
            return; // 메서드 종료
        }

        else if (closestTarget != null)
        {
            Vector2 target = (closestTarget.position - this.transform.position).normalized; // 타겟 벡터값 설정
            lookDirection = target; // 발견된 타겟을 바라보게 설정
            isAttacking = true; // 타겟 공격
        }

        else isAttacking = false;
    }

    // 가장 가까운 적 탐색
    private Transform FindClosestTarget()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, wh.AttackRange, enemyLayer); // 공격 범위 내에서 적 검출
        Transform closest = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider2D target in targets) // 검출된 타겟들 정보 루프화
        {
            float distance = Vector2.Distance(transform.position, target.transform.position);

            if (distance < minDistance) // 가까운 적 설정 조건문
            {
                minDistance = distance;
                closest = target.transform;
            }
        }

        return closest; // 최종적으로 가장 가까운 적 위치를 반환
    }

    public override void Death()
    {
        base.Death();
        Debug.Log("Game Over.");
        //gameManager.GameOver(); // ???? ???? ??? 
    }

    // temp 메서드. 나중에 스킬 습득 조건 발동에 맞춰 변경 예정
    public void ShowSkillSelectorUI()
    {
        skillSelectorUI.Initialize(this);
    }

    public void EnableInvincibility()
    {
        hasInvincibilitySkill = true;
        invincibleTimer = 0f;
    }

    // public void OnLevelUp()
    // {
    //     //레벨업 할때, 스킬 ui 이벤트 발생
    //     OnSkillSelectionTriggered?.Invoke(this);
    // }
    private bool IsAnyKeyPressed(List<KeyCode> keys)
    {
        foreach (KeyCode key in keys)
        {
            if (Input.GetKey(key))
                return true;
        }
        return false;
    }
    
}