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
        if (collision.CompareTag(playerTag))
        {
            // Phase 2~4 구간에서만 데미지 적용 나중에 체력바 생기면 데미지 적용
            //if (currentPhase >= 1 && currentPhase <= 3)
            //{
            //    PlayerHealth player = collision.GetComponent<PlayerHealth>();
            //    if (player != null)
            //    {
            //        player.TakeDamage(damage);
            //    }
            //}
            Debug.Log("아야");
        }
    }
}
