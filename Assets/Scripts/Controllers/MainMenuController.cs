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
    public PlayerMovement playerMovement;
    public void Update()
    {
        Time.timeScale = 1;
    }

    public void PlayGame()
    {
        CacheObjects();
        SaveManager.ClearSavedGame();
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
        StartCoroutine(LoadSavedGame());
    }

    private IEnumerator LoadSavedGame()
    {
        Debug.Log("LoadSavedGame() called in scene: " + SceneManager.GetActiveScene().name);
        int sceneToLoad = SceneManager.GetActiveScene().buildIndex + 1;
        if (SaveManager.IsGameSaved())
        {
            SaveData data = SaveManager.LoadGame();
            // Save the loaded position to be used later in PlayerMovement script
            PlayerPrefs.SetFloat("LoadedPlayerPositionX", data.playerPosition.x);
            PlayerPrefs.SetFloat("LoadedPlayerPositionY", data.playerPosition.y);
            PlayerPrefs.SetFloat("LoadedPlayerPositionZ", data.playerPosition.z);
            sceneToLoad = data.currentScene != 0 ? data.currentScene : sceneToLoad;
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Single);
        loadingImage.gameObject.SetActive(true);
        asyncLoad.allowSceneActivation = false;
        while (!asyncLoad.isDone)
        {
            // Get the current loading progress and convert it into a range from 0 to 360 degrees.
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            float fillAmount = progress * 360f;

            // Set the loading circle to be filled in.
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
