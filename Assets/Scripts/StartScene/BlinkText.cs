using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkText : MonoBehaviour
{
    private Text textComponent;
    public float fadeDuration = 1.0f;

    private void Start()
    {
        textComponent = GetComponent<Text>();

        StartCoroutine(FadeInOut());
    }

    private IEnumerator FadeInOut()
    {
        float halfFadeDuration = fadeDuration / 2f;

        yield return Fade(0f, 1f, halfFadeDuration);

        yield return Fade(1f, 0f, halfFadeDuration);

        StartCoroutine(FadeInOut());
    }

    private IEnumerator Fade(float startOpacity, float endOpacity, float duration)
    {
        float elapsedTime = 0f;

        Color startColor = textComponent.color;
        startColor.a = startOpacity;
        textComponent.color = startColor;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / duration);
            float opacity = Mathf.Lerp(startOpacity, endOpacity, t);

            Color newColor = textComponent.color;
            newColor.a = opacity;
            textComponent.color = newColor;

            yield return null;
        }
    }
}
