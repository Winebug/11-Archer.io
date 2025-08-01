using UnityEngine;
using TMPro;
using System.Collections;

public class RoomManager : MonoBehaviour
{
    [Header("방 Prefabs")]
    public GameObject[] roomPrefabs;

    [Header("생성 위치")]
    public Transform spawnPosition;
    public Vector3 roomOffset = new Vector3(0, 20, 0);

    [Header("UI")]
    public TextMeshProUGUI roomText;
    public float textMoveDistance = 500f;
    public float textMoveDuration = 1.0f; // 기존보다 느리게

    private int currentRoomIndex = 0;
    private GameObject currentRoom;
    private bool canEnterNextRoom = false; // 문 앞에 있는지 여부

    public int CurrentRoomIndex => currentRoomIndex;

    void Start()
    {
        SpawnRoom();
    }

    void Update()
    {
        // 문 앞에 있을 때만 다음 방 이동 가능
        if (canEnterNextRoom && Input.GetKeyDown(KeyCode.Return))
        {
            GoToNextRoom();
        }
    }

    private void SpawnRoom()
    {
        Vector3 fixedSpawnPos = new Vector3(-12.23f, spawnPosition.position.y + (roomOffset.y * currentRoomIndex), 0);
        currentRoom = Instantiate(roomPrefabs[currentRoomIndex], fixedSpawnPos, Quaternion.identity);

        StartCoroutine(AnimateRoomText($"Room {currentRoomIndex + 1}"));
    }

    public void GoToNextRoom()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Destroy(currentRoom);

        currentRoomIndex++;
        if (player != null)
        {
            player.transform.position = spawnPosition.position + new Vector3(0, roomOffset.y * currentRoomIndex, 0);
        }
        if (currentRoomIndex < roomPrefabs.Length)
        {
            SpawnRoom();
        }
        else
        {
            ShowClearScreen();
        }
    }

    private IEnumerator AnimateRoomText(string text)
    {
        // 중앙에서 등장 (Fade In)
        roomText.text = text;
        roomText.alpha = 0f;
        roomText.rectTransform.anchoredPosition = new Vector2(-textMoveDistance, roomText.rectTransform.anchoredPosition.y);

        float elapsed = 0f;
        while (elapsed < textMoveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / textMoveDuration); // Ease In
            roomText.rectTransform.anchoredPosition = Vector2.Lerp(new Vector2(-textMoveDistance, roomText.rectTransform.anchoredPosition.y),
                                                                   new Vector2(0, roomText.rectTransform.anchoredPosition.y), t);
            roomText.alpha = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }

        // 중앙에서 오른쪽으로 사라짐 (Fade Out)
        elapsed = 0f;
        while (elapsed < textMoveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / textMoveDuration); // Ease Out
            roomText.rectTransform.anchoredPosition = Vector2.Lerp(new Vector2(0, roomText.rectTransform.anchoredPosition.y),
                                                                   new Vector2(textMoveDistance, roomText.rectTransform.anchoredPosition.y), t);
            roomText.alpha = Mathf.Lerp(1f, 0f, t);
            yield return null;
        }
    }

    public void ShowClearScreen()
    {
        StartCoroutine(AnimateRoomText("CLEAR!"));
    }

    // 🔹 문 앞 Trigger 감지
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canEnterNextRoom = true;
            Debug.Log("문 앞에 도착! Enter키로 다음 방으로 이동 가능");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canEnterNextRoom = false;
            Debug.Log("문에서 멀어짐, 이동 불가");
        }
    }
}
