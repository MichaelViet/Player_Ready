using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    public bool showMouseClickHover = false;
    private bool wasPlayerInside = false;
    public RayCastWeapon playerWeapon;
    private QuestSystem questSystem;
    private bool fadeInCalled = false;
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
        questSystem = FindObjectOfType<QuestSystem>();
    }

    private void Update()
    {
        if (playerTransforms != null && sharedMonologueCanvasGroup != null && !zoneDisabled)
        {
            Transform closestPlayer = GetClosestPlayer(playerTransforms);
            float distance = Vector3.Distance(closestPlayer.position, transform.position);
            bool isPlayerInside = distance <= radius;

            if (isPlayerInside && !wasPlayerInside)
            {
                currentZone = this;
                currentSentenceIndex = 0;

                if (!playerStop)
                {
                    StartCoroutine(AutoChangeSentence());
                }

                if (playerWeapon != null)
                {
                    playerWeapon.SetCanShoot(false);
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
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        ChangeSentence();
                    }
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
                if (playerWeapon != null)
                {
                    playerWeapon.SetCanShoot(true);
                }
            }

            wasPlayerInside = isPlayerInside;

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