using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MonologueZone : MonoBehaviour
{
    public static MonologueZone currentZone; // Поточна монологічна зона
    public CanvasGroup sharedMonologueCanvasGroup; // Група канвасу для монологу
    public TMP_Text sharedMonologueText; // Текст монологу
    public List<string> monologueSentences; // Список речень монологу
    public int currentSentenceIndex = 0; // Індекс поточного речення
    public float radius = 5f; // Радіус зони
    public List<Transform> playerTransforms; // Список трансформацій гравців
    public bool isZoneCompleted; // Прапорець, що позначає, чи зона завершена
    private bool zoneDisabled = false; // Прапорець, що позначає, чи вимкнена зона
    public int zoneIndex; // Індекс зони
    public bool playerStop; // Прапорець, що позначає, чи зупиняється гравець
    public Image mouseClickHover; // Зображення підказки про клік мишею
    public bool showMouseClickHover = false; // Прапорець, що позначає, чи показувати підказку про клік мишею
    private bool wasPlayerInside = false; // Прапорець, що позначає, чи гравець був всередині зони
    public RayCastWeapon playerWeapon; // Зброя гравця
    private QuestSystem questSystem; // Система квестів
    private bool interactionEnabled = false;
    private void Start()
    {
        if (sharedMonologueCanvasGroup != null)
        {
            sharedMonologueCanvasGroup.alpha = 0; // Приховуємо канвас монологу
        }

        if (mouseClickHover != null)
        {
            mouseClickHover.enabled = false; // Вимикаємо підказку про клік мишею
        }
        questSystem = FindObjectOfType<QuestSystem>(); // Ініціалізуємо систему квестів
        CameraOffsetAnimator.OnAnimationEnd += EnableInteraction;
    }

    private void Update()
    {
        if (interactionEnabled)
        {
            if (playerTransforms != null && sharedMonologueCanvasGroup != null && !zoneDisabled)
            {
                Transform closestPlayer = GetClosestPlayer(playerTransforms); // Знаходимо найближчого гравця
                float distance = Vector3.Distance(closestPlayer.position, transform.position); // Відстань до найближчого гравця
                bool isPlayerInside = distance <= radius; // Перевіряємо, чи гравець всередині зони

                if (isPlayerInside && !wasPlayerInside)
                {
                    currentZone = this;
                    currentSentenceIndex = 0;

                    if (!playerStop)
                    {
                        StartCoroutine(AutoChangeSentence()); // Запускаємо автоматичну зміну речень
                    }

                    if (playerWeapon != null)
                    {
                        playerWeapon.SetCanShoot(false); // Вимикаємо можливість стріляти гравця
                    }
                }

                if (isPlayerInside)
                {
                    if (sharedMonologueText != null && monologueSentences.Count > 0)
                    {
                        sharedMonologueText.text = monologueSentences[currentSentenceIndex]; // Встановлюємо текс монологу
                    }

                    sharedMonologueCanvasGroup.alpha = 1; // Відображаємо канвас монологу

                    if (Input.GetMouseButtonDown(0) && sharedMonologueText != null && playerStop)
                    {
                        if (!EventSystem.current.IsPointerOverGameObject())
                        {
                            ChangeSentence(); // Змінюємо речення при кліку мишею
                        }
                    }

                    if (mouseClickHover != null)
                    {
                        mouseClickHover.enabled = showMouseClickHover && currentZone == this; // Встановлюємо видимість підказки про клік мишею
                    }
                }
                else if (wasPlayerInside)
                {
                    currentZone = null;
                    sharedMonologueCanvasGroup.alpha = 0; // Приховуємо канвас монологу

                    if (!playerStop)
                    {
                        StopCoroutine(AutoChangeSentence()); // Зупиняємо автоматичну зміну речень
                    }

                    if (mouseClickHover != null)
                    {
                        mouseClickHover.enabled = false; // Вимикаємо підказку про клік мишею
                    }
                    if (playerWeapon != null)
                    {
                        playerWeapon.SetCanShoot(true); // Увімкнюємо можливість стріляти гравця
                    }
                }
                wasPlayerInside = isPlayerInside;
            }
        }
    }

    private void OnDestroy()
    {
        CameraOffsetAnimator.OnAnimationEnd -= EnableInteraction;
    }
    private void EnableInteraction()
    {
        interactionEnabled = true;
    }

    private Transform GetClosestPlayer(List<Transform> players)
    {
        Transform closest = null;
        float minDistance = Mathf.Infinity;
        foreach (Transform player in players)
        {
            // Обчислюємо відстань між поточним гравцем і позицією зони монологу
            float distance = Vector3.Distance(player.position, transform.position);

            // Якщо відстань менша за minDistance, оновлюємо значення minDistance та closest
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = player;
            }
        }
        // Повертаємо трансформ гравця, який знаходиться найближче до зони монологу
        return closest;
    }

    private IEnumerator AutoChangeSentence()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f); // Затримка перед зміною речення

            ChangeSentence(); // Зміна речення
        }
    }

    private void ChangeSentence()
    {
        currentSentenceIndex++;

        if (currentSentenceIndex >= monologueSentences.Count)
        {
            sharedMonologueCanvasGroup.alpha = 0;
            isZoneCompleted = true;
            radius = 0f;

            if (!playerStop)
            {
                StopCoroutine(AutoChangeSentence()); // Зупиняємо автоматичну зміну речень
            }

            if (zoneIndex == 0)
            {
                StartCoroutine(questSystem.FadeIn()); // Запускаємо ефект затемнення для системи квестів
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius); // Відображення границі зони у вигляді сфери
    }
}