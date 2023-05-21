using System.Collections;
using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    public CanvasGroup inventoryPanel;
    public float fadeDuration = 1f;
    private bool isVisible = false;
    public BasePauseMenu PauseMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (isVisible)
            {
                StartCoroutine(FadeOut());
            }
            else
            {
                StartCoroutine(FadeIn());
            }
            isVisible = !isVisible;
        }
    }

    public IEnumerator FadeIn()
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            inventoryPanel.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }
        inventoryPanel.alpha = 1;
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            inventoryPanel.alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));
            yield return null;
        }
        inventoryPanel.alpha = 0;
        PauseMenu.ToggleCursor(visible: false);
    }
}