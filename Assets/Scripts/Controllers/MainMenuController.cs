using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    public string gameScene;
    public Image loadingImage, circleImg;
    public Button loadButton;


    public TextMeshProUGUI pressE;

    public void Update()
    {
        Time.timeScale = 1;
    }
    public void PlayGame()
    {
        CacheObjects();
        SaveManager.ClearSavedGame();
        StartCoroutine(LoadScene());
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void CacheObjects()
    {
        Resources.UnloadUnusedAssets();
    }

    public void Start()
    {
        loadButton.interactable = SaveManager.IsGameSaved();
    }


    public void Load()
    {
        StartCoroutine(LoadAsync());
    }

    private IEnumerator LoadAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(gameScene, LoadSceneMode.Single);
        loadingImage.gameObject.SetActive(true);
        asyncLoad.allowSceneActivation = false;
        while (!asyncLoad.isDone)
        {
            // Отримуємо поточний прогрес завантаження і конвертуємо його в діапазон від 0 до 360 градусів.
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            float fillAmount = progress * 360f;

            // Задаємо кружечку відповідний заповнення.
            circleImg.fillAmount = fillAmount / 360f;

            if (asyncLoad.progress >= 0.9f)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    asyncLoad.allowSceneActivation = true;
                }
                pressE.gameObject.SetActive(true);
            }
            yield return null;
        }
    }


    IEnumerator LoadScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        loadingImage.gameObject.SetActive(true);
        asyncLoad.allowSceneActivation = false;
        while (!asyncLoad.isDone)
        {
            // Отримуємо поточний прогрес завантаження і конвертуємо його в діапазон від 0 до 360 градусів.
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            float fillAmount = progress * 360f;

            // Задаємо кружечку відповідний заповнення.
            circleImg.fillAmount = fillAmount / 360f;

            if (asyncLoad.progress >= 0.9f)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    asyncLoad.allowSceneActivation = true;
                }
                pressE.gameObject.SetActive(true);
            }
            yield return null;
        }
    }
}