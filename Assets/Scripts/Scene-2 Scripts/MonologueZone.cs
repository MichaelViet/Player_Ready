using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MonologueZone : MonoBehaviour
{
    public static MonologueZone currentZone;
    public AudioClip nextMusic;
    public CanvasGroup sharedMonologueCanvasGroup;
    public TMP_Text sharedMonologueText;
    public List<string> monologueSentences;
    public int currentSentenceIndex = 0;
    public float radius = 5f;
    public Transform playerTransform;
    public bool isZoneCompleted;
    private bool zoneDisabled = false;
    public int zoneIndex;
    public bool playerStop;
    public Image mouseClickHover;
    public bool showMouseClickHover = false;
    private HintManager hintManager;
    public bool hintShown = false; // Відслідковує, чи був показаний підказка
    private bool wasPlayerInside = false; // зберігає, чи був гравець у зоні на попередній кадрі
    private QuestSystem quest;
    private bool fadeInCalled = false;
    private LevelManager levelManager;
    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        hintManager = FindObjectOfType<HintManager>();
        quest = FindObjectOfType<QuestSystem>();
        if (sharedMonologueCanvasGroup != null)
        {
            sharedMonologueCanvasGroup.alpha = 0;
        }

        if (mouseClickHover != null)
        {
            mouseClickHover.enabled = false;
        }
    }

    private void Update()
    {
        if (playerTransform != null && sharedMonologueCanvasGroup != null && !zoneDisabled)
        {
            float distance = Vector3.Distance(playerTransform.position, transform.position);
            bool isPlayerInside = distance <= radius; // перевіряє, чи гравець у зоні на поточному кадрі

            if (isPlayerInside && !wasPlayerInside)
            {
                currentZone = this;
                currentSentenceIndex = 0;

                if (!playerStop)
                {
                    StartCoroutine(AutoChangeSentence());
                }
            }

            if (isPlayerInside)
            {
                if (sharedMonologueText != null && monologueSentences.Count > 0)
                {
                    sharedMonologueText.text = monologueSentences[currentSentenceIndex];
                }

                sharedMonologueCanvasGroup.alpha = 1;

                if (Input.GetMouseButtonDown(0) && sharedMonologueText != null && playerStop)
                {
                    ChangeSentence();
                }

                if (mouseClickHover != null)
                {
                    mouseClickHover.enabled = showMouseClickHover && currentZone == this;
                }
            }
            else if (wasPlayerInside)
            {
                currentZone = null;
                sharedMonologueCanvasGroup.alpha = 0;

                if (!playerStop)
                {
                    StopCoroutine(AutoChangeSentence());
                }

                if (mouseClickHover != null)
                {
                    mouseClickHover.enabled = false;
                }
            }
            if (isZoneCompleted && !hintShown)
            {
                if (zoneIndex == 0 && !fadeInCalled) // Перевіряємо, що це перша зона і корутина ще не була викликана
                {
                    quest.StartCoroutine(quest.FadeIn()); // Викликаємо корутину FadeIn з QuestSystem
                    fadeInCalled = true; // Оновлюємо, що корутина FadeIn була викликана
                }

                hintManager.ShowHint(0); // Викликаємо ShowHint з індексом 0
                hintShown = true; // Оновлюємо, що підказка була показана
            }

            wasPlayerInside = isPlayerInside; // зберігаємо, що було на поточному кадрі для порівняння з наступним
        }
    }

    private IEnumerator AutoChangeSentence()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.5f);

            ChangeSentence();
        }
    }

    private void ChangeSentence()
    {
        currentSentenceIndex++;

        // Якщо монолог дійшов до кінця, змініть музику
        if (currentSentenceIndex >= monologueSentences.Count)
        {
            sharedMonologueCanvasGroup.alpha = 0;
            isZoneCompleted = true;
            radius = 0f;

            // Переключіть музику в LevelManager
            if (zoneIndex == 0) // перевіряємо чи це зона з індексом 0
            {
                levelManager.levelMusic = nextMusic; // замінюємо музику в LevelManager
                levelManager.audioController.PlayAudio(levelManager.levelMusic, null, null); // відтворюємо нову музику
            }

            if (!playerStop)
            {
                StopCoroutine(AutoChangeSentence());
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}