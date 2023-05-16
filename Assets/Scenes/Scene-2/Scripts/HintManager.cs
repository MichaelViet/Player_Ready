using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class HintManager : MonoBehaviour
{
    [System.Serializable]
    public class Hint
    {
        public string text;
        public bool hasBeenShown;

        public Hint(string text)
        {
            this.text = text;
            this.hasBeenShown = false;
        }
    }

    public List<Hint> hints = new List<Hint>();
    public TMP_Text hintText;
    public CanvasGroup hintGroup;
    public float fadeInDuration = 1f;
    public float displayDuration = 3f;
    public float fadeOutDuration = 1f;

    // Викликайте цей метод, щоб показати підказку за його індексом
    public void ShowHint(int index)
    {
        if (index < 0 || index >= hints.Count) return;

        Hint hint = hints[index];
        if (!hint.hasBeenShown)
        {
            SetHint(hint.text);
            hint.hasBeenShown = true;
        }
    }

    private void SetHint(string hint)
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
