using System.Collections;
using UnityEngine;

public class AutoSaveCoroutine : MonoBehaviour
{
    public float saveInterval = 120f; // Інтервал збереження у секундах

    private LevelThreeController levelThreeController;
    private Coroutine saveCoroutine;

    private void Start()
    {
        levelThreeController = FindObjectOfType<LevelThreeController>();

        // Запускаємо корутину збереження
        saveCoroutine = StartCoroutine(AutoSave());
    }

    private IEnumerator AutoSave()
    {
        while (true)
        {
            yield return new WaitForSeconds(saveInterval);

            // Викликаємо метод збереження гри
            //levelThreeController.SavePlayerProgress();
        }
    }

    private void OnDestroy()
    {
        // Зупиняємо корутину при знищенні об'єкту
        if (saveCoroutine != null)
        {
            StopCoroutine(saveCoroutine);
        }
    }
}
