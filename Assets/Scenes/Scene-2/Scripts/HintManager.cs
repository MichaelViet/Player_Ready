using UnityEngine;
using TMPro;
using System.Collections;

public class HintManager : MonoBehaviour
{
    public TMP_Text hintText;
    public CanvasGroup hintGroup;
    public float fadeInDuration = 1f;
    public float displayDuration = 3f;
    public float fadeOutDuration = 1f;

    public void SetHint(string hint)
    {
        hintText.text = hint;
        StartCoroutine(DisplayHint());
    }

    private IEnumerator DisplayHint()
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            hintGroup.alpha = Mathf.Clamp01(elapsedTime / fadeInDuration);
            yield return null;
        }

        hintGroup.alpha = 1;
        yield return new WaitForSeconds(displayDuration);

        elapsedTime = 0;
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            hintGroup.alpha = Mathf.Clamp01(1 - (elapsedTime / fadeOutDuration));
            yield return null;
        }

        hintGroup.alpha = 0;
    }
}

