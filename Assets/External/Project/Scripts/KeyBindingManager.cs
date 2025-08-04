using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyBindingManager : MonoBehaviour
{
    [System.Serializable]
    public class KeyBindRow
    {
        public string actionName;              // 액션 이름 (예: "Up", "Down")
        public Button keyButton;               // 키 입력 받을 버튼
        public Text keyText;                   // 현재 키를 표시할 텍스트
        public List<KeyCode> keyCodes = new(); // 바인딩된 키 리스트
    }

    private static KeyBindingManager keyInstance;
    [SerializeField] private List<KeyBindRow> keyRows; // UI에 연결된 키 항목들
    [SerializeField] private GameObject panelSetting;

    private KeyBindRow currentListeningRow = null;
    private Dictionary<string, List<KeyCode>> keyBindings = new();

    // 싱글톤 설정 및 키 바인딩 데이터 로드
    private void Awake()
    {
        if (keyInstance != null && keyInstance != this)
        {
            Destroy(gameObject);
            return;
        }

        keyInstance = this;
        DontDestroyOnLoad(gameObject);
        LoadAllBindings();
    }

    // 키 UI 초기화 및 버튼 리스너 등록
    private void Start()
    {
        foreach (var row in keyRows)
        {
            string saved = PlayerPrefs.GetString($"Key_{row.actionName}", "");
            if (System.Enum.TryParse(saved, out KeyCode savedCode))
            {
                row.keyCodes = new List<KeyCode> { savedCode };
            }

            row.keyButton.onClick.AddListener(() => OnClickKeyButton(row));
            UpdateKeyText(row);
        }
    }

    // 키 입력 대기 중일 때 입력된 키를 바인딩
    private void Update()
    {
        if (currentListeningRow != null && Input.anyKeyDown)
        {
            foreach (KeyCode code in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(code))
                {
                    currentListeningRow.keyCodes = new List<KeyCode> { code };
                    UpdateKeyText(currentListeningRow);
                    keyBindings[currentListeningRow.actionName] = new List<KeyCode> { code };
                    currentListeningRow = null;
                    break;
                }
            }
        }
    }

    // 기본 키 바인딩들을 불러옴
    private void LoadAllBindings()
    {
        LoadBinding("Up", new List<KeyCode> { KeyCode.W, KeyCode.UpArrow });
        LoadBinding("Down", new List<KeyCode> { KeyCode.S, KeyCode.DownArrow });
        LoadBinding("Left", new List<KeyCode> { KeyCode.A, KeyCode.LeftArrow });
        LoadBinding("Right", new List<KeyCode> { KeyCode.D, KeyCode.RightArrow });
    }

    // 지정된 키 바인딩을 PlayerPrefs에서 로드하거나 기본값으로 설정
    private void LoadBinding(string key, List<KeyCode> defaultKeys)
    {
        List<KeyCode> result = new();
        string saved = PlayerPrefs.GetString(key, "");

        if (string.IsNullOrEmpty(saved))
        {
            result = defaultKeys;
        }
        else
        {
            foreach (string part in saved.Split(','))
            {
                if (System.Enum.TryParse(part, out KeyCode parsed))
                {
                    result.Add(parsed);
                }
            }

            if (result.Count == 0)
                result = defaultKeys;
        }

        keyBindings[key] = result;
    }

    // 특정 액션에 해당하는 키가 눌렸는지 확인
    public bool IsKeyPressed(string action)
    {
        if (!keyBindings.ContainsKey(action)) return false;

        foreach (KeyCode key in keyBindings[action])
        {
            if (Input.GetKey(key)) return true;
        }

        return false;
    }

    // 현재 입력된 방향키 조합을 벡터로 반환
    public Vector2 GetMovementInput()
    {
        Vector2 move = Vector2.zero;

        if (IsKeyPressed("Up")) move += Vector2.up;
        if (IsKeyPressed("Down")) move += Vector2.down;
        if (IsKeyPressed("Left")) move += Vector2.left;
        if (IsKeyPressed("Right")) move += Vector2.right;

        return move;
    }

    // 키 설정 버튼 클릭 시 키 입력 대기 상태로 전환
    private void OnClickKeyButton(KeyBindRow row)
    {
        currentListeningRow = row;
        row.keyText.text = "...";
    }

    // 현재 설정된 키를 버튼 텍스트에 반영
    private void UpdateKeyText(KeyBindRow row)
    {
        row.keyText.text = row.keyCodes.Count > 0 ? row.keyCodes[0].ToString() : "None";
    }

    // 현재 키 바인딩들을 저장
    public void SaveKeyBindings()
    {
        foreach (var row in keyRows)
        {
            if (row.keyCodes.Count > 0)
            {
                keyBindings[row.actionName] = row.keyCodes;
                PlayerPrefs.SetString($"Key_{row.actionName}", row.keyCodes[0].ToString());
            }
        }

        PlayerPrefs.Save();
        Debug.Log("키 설정 저장 완료");
        panelSetting.SetActive(false);
    }

    // 외부에서 바인딩을 직접 설정
    public void SetBinding(string action, List<KeyCode> keys)
    {
        keyBindings[action] = keys;
        SaveBinding(action, keys);
    }

    // 지정된 키 바인딩을 저장
    private void SaveBinding(string key, List<KeyCode> keys)
    {
        string result = string.Join(",", keys);
        PlayerPrefs.SetString(key, result);
        PlayerPrefs.Save();
    }
}
