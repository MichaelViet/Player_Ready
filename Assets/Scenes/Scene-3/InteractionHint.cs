using UnityEngine;

public class InteractionHint : MonoBehaviour
{
    public Transform playerTransform;
    public float interactionRadius = 3f;
    public GameObject pressEPrefab;
    private GameObject pressEInstance;

    void Start()
    {
        // Створюємо інстанцію PressE віджета і відразу вимикаємо його
        pressEInstance = Instantiate(pressEPrefab, transform.position, Quaternion.identity, transform);
        pressEInstance.SetActive(false);
    }

    void Update()
    {
        // Якщо гравець в радіусі взаємодії, показуємо віджет PressE
        if (Vector3.Distance(transform.position, playerTransform.position) <= interactionRadius)
        {
            pressEInstance.SetActive(true);
        }
        else
        {
            pressEInstance.SetActive(false);
        }
    }
}
