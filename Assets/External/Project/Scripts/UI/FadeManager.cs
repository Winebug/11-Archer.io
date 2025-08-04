using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public Image titleImage;

    public float fadeDuration = 1f; // 페이드 아웃 시간

    void Start()
    {
        // 씬이 시작될 때 바로 페이드 아웃 시작
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float timer = 0f;
        Color startColor = titleImage.color; // 현재 titleImage 색상 (투명 or 현재 색상)
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1f); // 불투명하게 만들 색상 (알파 1)

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / fadeDuration;
            titleImage.color = Color.Lerp(startColor, endColor, progress);
            yield return null;
        }
        titleImage.color = endColor;
    }
}