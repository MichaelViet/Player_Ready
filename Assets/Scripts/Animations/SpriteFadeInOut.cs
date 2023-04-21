using System.Collections;
using UnityEngine;

public class SpriteFadeInOut : MonoBehaviour
{
    public float fadeDuration = 1.0f;
    public float waitDuration = 0.5f;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Color color = spriteRenderer.color;
        color.a = 0f;
        spriteRenderer.color = color;
        StartCoroutine(FadeInOut());
    }
    private IEnumerator FadeInOut()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitDuration);
            // З'явлення альфа-компоненту
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                Color color = spriteRenderer.color;
                color.a = Mathf.Lerp(0, 1, t / fadeDuration);
                spriteRenderer.color = color;
                yield return null;
            }
            yield return new WaitForSeconds(waitDuration);
            // Зникнення альфа-компоненту
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                Color color = spriteRenderer.color;
                color.a = Mathf.Lerp(1, 0, t / fadeDuration);
                spriteRenderer.color = color;
                yield return null;
            }
        }
    }
}
