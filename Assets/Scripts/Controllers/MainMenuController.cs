using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Індекс поточного завантаженного рівня
    public void PlayGame()
    {
        // Кешування всіх невикористаних ресурсів
        CacheObjects();

        // Завантажити наступну сцену в індекс збірки
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void CacheObjects()
    {
        // Кешувати всі невикористані ресурси
        Resources.UnloadUnusedAssets();
    }
}
