using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MonologueZone : MonoBehaviour
{
    public static MonologueZone currentZone;
    public CanvasGroup sharedMonologueCanvasGroup;
    public TMP_Text sharedMonologueText;
    public List<string> monologueSentences;
    public int currentSentenceIndex = 0;
    public float radius = 5f;
    public List<Transform> playerTransforms;
    public bool isZoneCompleted;
    private bool zoneDisabled = false;
    public int zoneIndex;
    public bool playerStop;
    public Image mouseClickHover;
    public bool showMouseClickHover = false; // по замовчуванню - вимкнуто
    private bool wasPlayerInside = false; // зберігає, чи був гравець у зоні на попередній кадрі

    private void Start()
    {
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
        if (playerTransforms != null && sharedMonologueCanvasGroup != null && !zoneDisabled)
        {
            Transform closestPlayer = GetClosestPlayer(playerTransforms);
            float distance = Vector3.Distance(closestPlayer.position, transform.position);
            bool isPlayerInside = distance <= radius; // перевіряє, чи гравець у зоні на поточному кадрі

            if (isPlayerInside && !wasPlayerInside)
            {
                // якщо гравець тільки заходить у зону
                currentZone = this;
                currentSentenceIndex = 0;

                if (!playerStop)
                {
                    StartCoroutine(AutoChangeSentence());
                }
            }

            if (isPlayerInside)
            {
                // якщо гравець перебуває у зоні
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
                // якщо гравець тільки виходить із зони
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

            wasPlayerInside = isPlayerInside; // зберігаємо, що було на поточному кадрі для порівняння з наступним
        }
    }
    private Transform GetClosestPlayer(List<Transform> players)
    {
        Transform closest = null;
        float minDistance = Mathf.Infinity;

        foreach (Transform player in players)
        {
            float distance = Vector3.Distance(player.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = player;
            }
        }

        return closest;
    }

    private IEnumerator AutoChangeSentence()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);

            ChangeSentence();
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