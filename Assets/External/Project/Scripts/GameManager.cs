using UnityEngine;

// 게임 전체를 관리하는 메인 매니저 클래스
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Player player { get; private set; } // 플레이어 컨트롤러 (읽기 전용 프로퍼티)
    private UnitController _unitController;

    [SerializeField] private int currentWaveIndex = 0; // 현재 웨이브 번호

    // 컴패니언 생성 및 관리하는 매니저
    private CompanionManager companionManager;

    private void Awake()
    {
        // 싱글톤 할당
        instance = this;

        // 플레이어 찾고 초기화
        player = FindObjectOfType<Player>();
        player.Init(this);

        // 컴패니언 매니저 초기화
        companionManager = GetComponentInChildren<CompanionManager>();
        companionManager.Init(this);
    }

    public void StartGame()
    {
        StartNextWave(); // 첫 웨이브 시작
    }

    void StartNextWave()
    {
        // 컴패니언 소환
        companionManager.SpawnCompanion();

        // 현재 웨이브 인덱스 증가 및 필요시 난이도 조정
    }

    // 웨이브 종료 후 다음 웨이브 시작
    public void EndOfWave()
    {
        StartNextWave();
    }

    // 플레이어가 죽었을 때 게임 오버 처리
    public void GameOver()
    {
        // 스폰 중지
    }

    // 개발용 테스트: Space 키로 게임 시작
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
    }
}