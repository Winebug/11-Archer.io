using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneUI : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Slider soundSlider;

    private void Start()
    {
        // 버튼 이벤트
        startButton.onClick.AddListener(OnStartClicked);
        exitButton.onClick.AddListener(OnExitClicked);
        settingButton.onClick.AddListener(OnSettingClicked);

        // 슬라이더 초기값과 이벤트 등록
        soundSlider.value = UIManager.Instance.Volume;
        soundSlider.onValueChanged.AddListener(UIManager.Instance.SetVolume);
    }

    private void OnStartClicked()
    {
        SceneManager.LoadScene("SampleScene"); //나중에 씬이름 변경시 이 코드도 변경
    }

    private void OnExitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    private void OnSettingClicked()
    {
        Debug.Log("세팅은 아직 미구현");
    }
}
