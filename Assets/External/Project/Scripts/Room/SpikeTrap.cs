using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [Header("트랩 설정")]
    public Sprite[] phases;          // Phase 1~4 스프라이트 (순서대로)
    public float[] phaseDurations;   // 각 Phase 유지 시간 (초)
    public int damage = 10;          // 가시 데미지
    public string playerTag = "Player"; // 플레이어 태그

    private SpriteRenderer sr;
    private int currentPhase = 0;
    private float timer = 0f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        // 초기 스프라이트 설정
        if (phases.Length > 0)
            sr.sprite = phases[0];
    }

    void Update()
    {
        if (phases.Length == 0 || phaseDurations.Length != phases.Length) return;

        timer += Time.deltaTime;

        // 현재 Phase 시간 경과 시 다음 Phase로 이동
        if (timer >= phaseDurations[currentPhase])
        {
            timer = 0f;
            currentPhase = (currentPhase + 1) % phases.Length;
            sr.sprite = phases[currentPhase];
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // 플레이어 태그와 충돌한 경우만 처리
        if (collision.CompareTag(playerTag))
        {
            // UnitController 가져오기
            UnitController unitController = collision.GetComponent<UnitController>();
            if (unitController == null)
            {
                Debug.LogWarning($"UnitController가 없습니다: {collision.name}");
                return;
            }

            // Phase 2~4 구간에서만 데미지 적용
            if (currentPhase >= 1 && currentPhase <= 3)
            {
                // ChangeHealth를 통해 체력 감소 (예: -10 데미지)
                bool damaged = unitController.ChangeHealth(-10f);
                if (damaged)
                {
                    Debug.Log($"{collision.name}가 스파이크 트랩에서 데미지를 입음");
                }
            }
        }
    }
}
