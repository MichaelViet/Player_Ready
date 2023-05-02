using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    public Image loadingImage, circleImg;
    public Button loadButton;
    public TextMeshProUGUI pressE;
    public AudioController audioController;
    public TrainAnimation trainAnimation;
    public void Update()
    {
        Time.timeScale = 1;
    }

    public void PlayGame()
    {

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
        Resources.UnloadUnusedAssets();
    }

    public void Start()
    {
        loadButton.interactable = SaveManager.IsGameSaved();
    }

    public void Load()
    {
        trainAnimation.StopTrainAnimation();
        audioController.musicSource.Stop();
        StartCoroutine(LoadSavedGame());
    }

    private IEnumerator LoadSavedGame()
    {
        Debug.Log("LoadSavedGame() called in scene: " + SceneManager.GetActiveScene().name);
        int sceneToLoad = SceneManager.GetActiveScene().buildIndex + 1;
        if (SaveManager.IsGameSaved())
        {
            SaveData data = SaveManager.LoadGame();
            PlayerPrefs.SetFloat("LoadedWizardPositionX", data.wizardPosition.x);
            PlayerPrefs.SetFloat("LoadedWizardPositionY", data.wizardPosition.y);
            PlayerPrefs.SetFloat("LoadedWizardPositionZ", data.wizardPosition.z);
            PlayerPrefs.SetFloat("LoadedPlayerPositionX", data.playerPosition.x);
            PlayerPrefs.SetFloat("LoadedPlayerPositionY", data.playerPosition.y);
            PlayerPrefs.SetFloat("LoadedPlayerPositionZ", data.playerPosition.z);
            PlayerPrefs.SetInt("LoadedCurrentDialogIndex", data.currentDialogIndex);
            PlayerPrefs.SetInt("LoadedCurrentSentenceIndex", data.currentSentenceIndex);
            DialogReader dialogReader = FindObjectOfType<DialogReader>();
            if (dialogReader != null)
            {
                dialogReader.DisplayDialog();
            }
            sceneToLoad = data.currentScene != 0 ? data.currentScene : sceneToLoad;
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Single);
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

    IEnumerator LoadNewGame()
    {
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
