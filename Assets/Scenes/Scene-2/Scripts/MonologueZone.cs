using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MonologueZone : MonoBehaviour
{
    public CanvasGroup monologueCanvasGroup;
    public TMP_Text monologueText;
    public List<string> monologueSentences;
    public int currentSentenceIndex = 0;
    public float radius = 5f;
    public Transform playerTransform;
    private bool zoneDisabled = false;

    private void Start()
    {
        if (monologueCanvasGroup != null)
        {
            monologueCanvasGroup.alpha = 0;
        }

        if (monologueText != null && monologueSentences.Count > 0)
        {
            monologueText.text = monologueSentences[currentSentenceIndex];
        }
    }

    private void Update()
    {
        if (playerTransform != null && monologueCanvasGroup != null && !zoneDisabled)
        {
            float distance = Vector3.Distance(playerTransform.position, transform.position);

            if (distance <= radius)
            {
                monologueCanvasGroup.alpha = 1;

                if (Input.GetMouseButtonDown(0) && monologueText != null)
                {
                    currentSentenceIndex++;

                    if (currentSentenceIndex >= monologueSentences.Count)
                    {
                        monologueCanvasGroup.alpha = 0;
                        zoneDisabled = true;
                    }
                    else
                    {
                        monologueText.text = monologueSentences[currentSentenceIndex];
                    }
                }
            }
            else
            {
                monologueCanvasGroup.alpha = 0;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
