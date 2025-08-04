using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private AudioSource bgmSource;

    private const string VolumeKey = "MasterVolume";
    public float Volume { get; private set; } = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Volume = PlayerPrefs.GetFloat(VolumeKey, 1f);
        AudioListener.volume = Volume;

        bgmSource.volume = Volume;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void SetVolume(float value)
    {
        Volume = value;
        AudioListener.volume = value;
        PlayerPrefs.SetFloat(VolumeKey, value);
    }
    public void StopBGM()
    {
        if (bgmSource != null && bgmSource.isPlaying)
            bgmSource.Stop();
    }
    public void OnReturnClicked()
    {
        SceneManager.LoadScene("StartScene");
    }
}

