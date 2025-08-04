using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField][Range(0f, 1f)] private float soundEffectVolume; // 효과음 볼륨
    [SerializeField][Range(0f, 1f)] private float soundEffectPitchVariance; // 효과음 피치 랜덤성
    [SerializeField][Range(0f, 1f)] private float musicVolume; // 배경 음악 볼륨

    private AudioSource musicAudioSource; // 배경 음악용 AudioSource

    // BGM 배열
    [SerializeField] private AudioClip[] bgmArray;

    public AudioClip musicClip; // 기본 배경 음악 클립

    public SoundSource soundSourcePrefab; // 효과음을 재생할 프리팹 (SoundSource 사용)
    private void Awake()
    {
        instance = this;

        // 배경음 재생용 AudioSource 설정
        musicAudioSource = GetComponent<AudioSource>();
        musicAudioSource.volume = musicVolume;
        musicAudioSource.loop = true;
    }

    private void Start()
    {
        // BGM 배열에서 랜덤 선택
        AudioClip randomBGM = bgmArray[Random.Range(0, bgmArray.Length)];

        // 기본 배경 음악 시작
        ChangeBackGroundMusic(randomBGM);
    }

    // 배경 음악을 다른 클립으로 교체하는 함수
    public void ChangeBackGroundMusic(AudioClip clip)
    {
        musicAudioSource.Stop();
        musicAudioSource.clip = clip;
        musicAudioSource.Play();
    }

    // 효과음을 재생하는 정적 메서드 (외부 어디서든 호출 가능)
    public static void PlayClip(AudioClip clip)
    {
        // SoundSource 프리팹 인스턴스 생성 후 재생
        //SoundSource obj = Instantiate(instance.soundSourcePrefab);
        //SoundSource soundSource = obj.GetComponent<SoundSource>();
       // soundSource.Play(clip, instance.soundEffectVolume, instance.soundEffectPitchVariance);
    }
}