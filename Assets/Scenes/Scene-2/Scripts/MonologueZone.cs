using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MonologueZone : MonoBehaviour
{
    public static MonologueZone currentZone;

    public CanvasGroup sharedMonologueCanvasGroup;
    public TMP_Text sharedMonologueText;
    public List<string> monologueSentences;
    public int currentSentenceIndex = 0;
    public float radius = 5f;
    public Transform playerTransform;
    public bool isZoneCompleted;
    private bool zoneDisabled = false;
    public int zoneIndex;
    private void Start()
    {
        if (sharedMonologueCanvasGroup != null)
        {
            sharedMonologueCanvasGroup.alpha = 0;
        }
    }

    private void Update()
    {
        if (playerTransform != null && sharedMonologueCanvasGroup != null && !zoneDisabled)
        {
            float distance = Vector3.Distance(playerTransform.position, transform.position);

            if (distance <= radius)
            {
                if (currentZone != this)
                {
                    currentZone = this;
                    currentSentenceIndex = 0;
                }

                if (sharedMonologueText != null && monologueSentences.Count > 0)
                {
                    sharedMonologueText.text = monologueSentences[currentSentenceIndex];
                }

                sharedMonologueCanvasGroup.alpha = 1;

                if (Input.GetMouseButtonDown(0) && sharedMonologueText != null)
                {
                    currentSentenceIndex++;

                    if (currentSentenceIndex >= monologueSentences.Count)
                    {
                        sharedMonologueCanvasGroup.alpha = 0;
                        isZoneCompleted = true;
                        radius = 0f;
                    }
                }
            }
            else
            {
                if (currentZone == this)
                {
                    currentZone = null;
                    sharedMonologueCanvasGroup.alpha = 0;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
