using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneUI : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button tutorialButton;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private GameObject panelSetting;

    private void OnStartClicked()
    {
        UIManager.Instance.StopBGM();
        SceneManager.LoadScene("MainScene"); //나중에 씬이름 변경시 이 코드도 변경
        //SceneManager.LoadScene("SkillTestTemp2"); //나중에 씬이름 변경시 이 코드도 변경
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
        panelSetting.SetActive(true);
    }

    private void OnTutorialClicked()
    {
        UIManager.Instance.StopBGM();
        SceneManager.LoadScene("Tutorial"); //나중에 씬이름 변경시 이 코드도 변경
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬이 로드된 후 필요한 오브젝트를 찾아 다시 참조. 이게 없으면 참조가 깨져서 버튼이 클릭이 안됨
        startButton = GameObject.Find("StartBtn")?.GetComponent<Button>();
        if (startButton != null)
        {
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(OnStartClicked);
        }
        exitButton = GameObject.Find("ExitBtn")?.GetComponent<Button>();
        if(exitButton != null)
        {
            exitButton.onClick.RemoveAllListeners();
            exitButton.onClick.AddListener(OnExitClicked);
        }
        settingButton = GameObject.Find("SettingBtn")?.GetComponent<Button>();
        if (settingButton != null)
        {
            settingButton.onClick.RemoveAllListeners();
            settingButton.onClick.AddListener(OnSettingClicked);
        }
        tutorialButton = GameObject.Find("TutorialBtn")?.GetComponent<Button>();
        if (tutorialButton != null)
        {
            tutorialButton.onClick.RemoveAllListeners();
            tutorialButton.onClick.AddListener(OnTutorialClicked);
        }
        soundSlider = GameObject.Find("SoundSlider")?.GetComponent<Slider>();
        if (soundSlider != null)
        {
            soundSlider.onValueChanged.RemoveAllListeners();
            soundSlider.value = UIManager.Instance.Volume;
            soundSlider.onValueChanged.AddListener(UIManager.Instance.SetVolume);
        }

        GameObject canvas = GameObject.FindWithTag("Canvas");

        if (canvas != null)
        {
            Transform canvasTransform = canvas.transform;
            Transform pausePanelTransform = canvasTransform.Find("Panel_Setting");

            if (pausePanelTransform != null)
            {
                panelSetting = pausePanelTransform.gameObject;
                panelSetting.SetActive(false);
            }
        }

    }
}
