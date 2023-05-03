using System.Collections;
using UnityEngine;

public class TreeDestruction : MonoBehaviour
{
    [SerializeField] private float interactionRadius = 2f;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float timeToRotate = 2f;
    [SerializeField] private float startRotationZ = -47.26f;
    [SerializeField] private float endRotationZ = -83.6f;
    public GameObject emptyWall;
    public GameObject canvasE;
    private bool isDestroyed = false;

    private void Update()
    {
        bool withinInteractionRadius = CheckPlayerDistance();

        if (Input.GetKeyDown(KeyCode.E) && withinInteractionRadius && !isDestroyed)
        {
            StartCoroutine(DestroyTree());
            interactionRadius = 0f; // змінюємо значення interactionRadius на 0
        }

        // Вимкніть buttonE, коли гравець натисне кнопку E або вийде з радіусу interactionRadius
        if (canvasE.activeSelf && (!withinInteractionRadius || Input.GetKeyDown(KeyCode.E)))
        {
            canvasE.SetActive(false);
        }
    }

    private bool CheckPlayerDistance()
    {
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        bool withinInteractionRadius = distance <= interactionRadius;

        // Включіть buttonE, коли гравець знаходиться всередині interactionRadius
        if (withinInteractionRadius && !canvasE.activeSelf)
        {
            canvasE.SetActive(true);
        }

        return withinInteractionRadius;
    }

    private IEnumerator DestroyTree()
    {
        isDestroyed = true;
        float elapsedTime = 0;

        while (elapsedTime < timeToRotate)
        {
            float zRotation = Mathf.Lerp(startRotationZ, endRotationZ, elapsedTime / timeToRotate);
            transform.rotation = Quaternion.Euler(0, 0, zRotation);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0, 0, endRotationZ);
        emptyWall.SetActive(false);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
