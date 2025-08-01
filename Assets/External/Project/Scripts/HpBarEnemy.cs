using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HpBarEnemy : MonoBehaviour
{
    [SerializeField] private Slider hpBar;
    [SerializeField] private Enemy enemy;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private Vector2 offset = new Vector2(0, 1f);
    [SerializeField] private Transform target;
    [SerializeField] private RectTransform hpbarPos;
    [SerializeField] private Camera mainCamera;

    private Canvas canvas;
    private float targetValue;
    private float lerpSpeed = 5f;

    void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        canvas = GetComponentInParent<Canvas>();
        targetValue = enemy.CurrentHealth;
        UpdateHealthBar();
        UpdatePosition();  
    }

    void Update()
    {
        UpdateHealthBar();
        UpdatePosition();
    }

    private void UpdateHealthBar()
    {
        hpBar.maxValue = enemy.Health;
        hpBar.value = enemy.CurrentHealth;
        hpBar.value = Mathf.Lerp(hpBar.value, targetValue, Time.deltaTime * lerpSpeed);
        hpText.text = $"{Mathf.FloorToInt(enemy.CurrentHealth)}";
    }

    private void UpdatePosition()
    {
        Vector3 worldPosition = target.position + (Vector3)offset;
        Vector2 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);
        Vector2 canvasPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            screenPosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : mainCamera,
            out canvasPosition
        );
        hpbarPos.anchoredPosition = canvasPosition;
    }
}