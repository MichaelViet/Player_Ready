using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class MainMenuController : MonoBehaviour
{
    // Декларуємо компоненти інтерфейсу користувача та аудіо-контролера
    public Image loadingImage, circleImg;
    public Button loadButton;
    public TextMeshProUGUI pressE;
    public AudioController audioController;
    public TrainAnimation trainAnimation;

    public void Start()
    {
        // Налаштування курсору при запуску
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Відключити кнопку завантаження, якщо гра не збережена
        loadButton.interactable = SaveManager.IsGameSaved();
    }
    public void Update()
    {
        Time.timeScale = 1;
    }

    public void PlayGame()
    {
        // Запускаємо гру: очищуємо кеш, зупиняємо аудіо та анімацію, завантажуємо нову гру
        CacheObjects();
        SaveManager.ClearSavedGame();
        audioController.musicSource.Stop();
        trainAnimation.StopTrainAnimation();
        StartCoroutine(LoadNewGame());
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void CacheObjects()
    {
        // Очищення кешу ресурсів
        Resources.UnloadUnusedAssets();
    }

    public void Load()
    {
        // Завантаження збереженої гри: зупиняємо анімацію і аудіо, а потім завантажуємо гру
        trainAnimation.StopTrainAnimation();
        audioController.musicSource.Stop();
        StartCoroutine(LoadSavedGame());
    }

    private IEnumerator LoadSavedGame()
    {
        int sceneToLoad = SceneManager.GetActiveScene().buildIndex + 1;  // Змінна для визначення наступної сцени для завантаження
        SaveData data = SaveManager.LoadGame(); // Завантажуємо дані збереження
        if (SaveManager.IsGameSaved())
        {
            // Перевірка на Scene-2
            if (data.currentScene == 2)
            {
                PlayerPrefs.SetFloat("LoadedWizzardPositionX", data.wizzardPosition.x);
                PlayerPrefs.SetFloat("LoadedWizzardPositionY", data.wizzardPosition.y);
                PlayerPrefs.SetFloat("LoadedWizzardPositionZ", data.wizzardPosition.z);

                PlayerPrefs.SetFloat("LoadedPlayerPositionX", data.playerPosition.x);
                PlayerPrefs.SetFloat("LoadedPlayerPositionY", data.playerPosition.y);
                PlayerPrefs.SetFloat("LoadedPlayerPositionZ", data.playerPosition.z);

                PlayerPrefs.SetInt("LoadedCurrentDialogIndex", data.currentDialogIndex);
                PlayerPrefs.SetInt("LoadedCurrentSentenceIndex", data.currentSentenceIndex);

                PlayerPrefs.Save();
                DialogReader dialogReader = FindObjectOfType<DialogReader>();
                if (dialogReader != null)
                {
                    dialogReader.SetCurrentDialogIndex(PlayerPrefs.GetInt("LoadedCurrentDialogIndex"));
                    dialogReader.SetCurrentSentenceIndex(PlayerPrefs.GetInt("LoadedCurrentSentenceIndex"));
                    dialogReader.DisplayDialog();
                }
            }
            if (data.currentScene == 3) // Перевірка на Scene-3
            {
                PlayerPrefs.SetFloat("PlayerPositionX", data.playerMotorPosition.x);
                PlayerPrefs.SetFloat("PlayerPositionY", data.playerMotorPosition.y);
                PlayerPrefs.SetFloat("PlayerPositionZ", data.playerMotorPosition.z);

                DialogReader dialogReader = FindObjectOfType<DialogReader>();
                if (dialogReader != null)
                {
                    dialogReader.SetCurrentDialogIndex(PlayerPrefs.GetInt("LoadedCurrentDialogIndex"));
                    dialogReader.SetCurrentSentenceIndex(PlayerPrefs.GetInt("LoadedCurrentSentenceIndex"));
                    dialogReader.DisplayDialog();
                }
            }
            if (data.currentScene == 4) // Перевірка на Scene-4
            {
                PlayerPrefs.SetFloat("LoadedWizzardPositionX", data.wizzardPosition.x);
                PlayerPrefs.SetFloat("LoadedWizzardPositionY", data.wizzardPosition.y);
                PlayerPrefs.SetFloat("LoadedWizzardPositionZ", data.wizzardPosition.z);

                PlayerPrefs.SetFloat("LoadedPlayerPositionX", data.playerPosition.x);
                PlayerPrefs.SetFloat("LoadedPlayerPositionY", data.playerPosition.y);
                PlayerPrefs.SetFloat("LoadedPlayerPositionZ", data.playerPosition.z);

                PlayerPrefs.SetInt("LoadedCurrentDialogIndex", data.currentDialogIndex);
                PlayerPrefs.SetInt("LoadedCurrentSentenceIndex", data.currentSentenceIndex);

                DialogReader dialogReader = FindObjectOfType<DialogReader>();
                if (dialogReader != null)
                {
                    dialogReader.SetCurrentDialogIndex(PlayerPrefs.GetInt("LoadedCurrentDialogIndex"));
                    dialogReader.SetCurrentSentenceIndex(PlayerPrefs.GetInt("LoadedCurrentSentenceIndex"));
                    dialogReader.DisplayDialog();
                }
            }
            sceneToLoad = data.currentScene != 0 ? data.currentScene : sceneToLoad; // Вибираємо сцену для завантаження

            // Завантажуємо сцену асинхронно
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Single);
            loadingImage.gameObject.SetActive(true);
            asyncLoad.allowSceneActivation = false;
            while (!asyncLoad.isDone) // Показуємо загрузку
            {
                float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
                float fillAmount = progress * 360f;
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


    IEnumerator LoadNewGame()
    {
        // Завантажуємо нову гру
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        loadingImage.gameObject.SetActive(true);
        asyncLoad.allowSceneActivation = false;
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            float fillAmount = progress * 360f;
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
