using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class QuestSystem : MonoBehaviour
{
    public List<Quest> questList;
    public GameObject player;
    public CanvasGroup questPanel;
    public TMP_Text questText;
    public TMP_Text DescriptionQuest;
    public float fadeInDuration = 1f;
    public float fadeOutDuration = 1f;
    public float displayDuration = 3f;
    private float fadeOutDelay;
    private bool isPanelVisible;

    private void Start()
    {
        foreach (var quest in questList)
        {
            if (quest.TriggerObject != null)
                quest.TriggerObject.enabled = false; // Вимкнути всі квести на початку
            quest.OnQuestComplete += HandleQuestComplete;
        }

        // Activate the first quest
        if (questList.Count > 0)
        {
            questList[0].IsActive = true;
            if (questList[0].TriggerObject != null)
                questList[0].TriggerObject.enabled = true;
        }
    }

    public Quest GetActiveQuest()
    {
        foreach (var quest in questList)
        {
            if (quest.IsActive && !quest.IsComplete)
            {
                return quest;
            }
        }
        return null; // Повертає null, якщо активний квест не знайдено
    }

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

        UpdateQuestText();
    }

    private void HandleQuestComplete(Quest quest)
    {
        Debug.Log($"Квест {quest.Name} виповнено!");
        quest.IsActive = false;
        if (quest.TriggerObject != null)
            quest.TriggerObject.enabled = false;

        // Активувати наступний квест, якщо він існує
        int currentIndex = questList.IndexOf(quest);
        if (currentIndex + 1 < questList.Count)
        {
            questList[currentIndex + 1].IsActive = true;
            if (questList[currentIndex + 1].TriggerObject != null)
                questList[currentIndex + 1].TriggerObject.enabled = true;
        }
    }

    private void UpdateQuestText()
    {
        questText.text = ""; // Очищення тексту квесту
        DescriptionQuest.text = ""; // Очищення тексту опису квесту
        foreach (var quest in questList)
        {
            if (quest.IsActive)
            {
                string questStatus = quest.IsComplete ? "Виповнено!" : "- В процесі виконання";
                questText.text = $"{quest.Name}: {questStatus}\n";
                DescriptionQuest.text = quest.Description; // Встановлення тексту опису
                break;
            }
        }
    }

    public IEnumerator FadeIn()
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

    public IEnumerator FadeOut()
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

    private void OnDestroy()
    {
        foreach (var quest in questList)
        {
            quest.OnQuestComplete -= HandleQuestComplete;
        }
    }

    public void StartQuest(Quest quest)
    {
        quest.IsActive = true;
        quest.TriggerObject.enabled = true;
        StartCoroutine(FadeIn());
    }

    public void CompleteQuest(Quest quest)
    {
        if (quest.IsActive && !quest.IsComplete)
        {
            quest.IsComplete = true;
            quest.TriggerObject.enabled = false;
            HandleQuestComplete(quest);
        }
    }
}


[System.Serializable]
public class Quest
{
    public string Name;
    public string Description;
    public bool IsActive;
    public bool IsComplete;
    public MonoBehaviour TriggerObject;
    public event Action<Quest> OnQuestComplete;
}
