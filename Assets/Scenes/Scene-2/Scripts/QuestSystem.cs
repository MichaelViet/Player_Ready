using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class QuestSystem : MonoBehaviour
{
    public MonologueZone monologueZone;
    public WizardController wizardController;
    public bool questIsActive = false;
    public bool questIsComplete = false;
    public string questName;
    public TMP_Text questText;
    public CanvasGroup questPanel;
    public float fadeInDuration = 0.5f;
    public float fadeOutDuration = 1f;
    public float displayDuration = 5f;
    private float fadeOutDelay;
    private bool isPanelVisible;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isPanelVisible)
        {
            StartCoroutine(FadeIn());
        }

        if (isPanelVisible)
        {
            fadeOutDelay -= Time.deltaTime;
            if (fadeOutDelay <= 0)
            {
                StartCoroutine(FadeOut());
            }
        }
        if (!questIsActive && monologueZone.isZoneCompleted)
        {
            questIsActive = true;
        }

        if (questIsActive && !questIsComplete && wizardController.dialogComplete)
        {
            questIsComplete = true;
            Debug.Log("Квест успішно завершено!");
        }

        UpdateQuestText();
    }

    private void UpdateQuestText()
    {
        if (questText != null)
        {
            string questStatus = questIsActive ? (questIsComplete ? "виповнено" : "в процесі") : "неактивний";
            questText.text = $"{questName}: {questStatus}";
        }
    }
    private IEnumerator FadeIn()
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            questPanel.alpha = Mathf.Clamp01(elapsedTime / fadeInDuration);
            yield return null;
        }
        questPanel.alpha = 1;
        isPanelVisible = true;
        fadeOutDelay = displayDuration;
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            questPanel.alpha = Mathf.Clamp01(1 - (elapsedTime / fadeOutDuration));
            yield return null;
        }
        questPanel.alpha = 0;
        isPanelVisible = false;
    }

}
