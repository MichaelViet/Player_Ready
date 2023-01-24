using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class MainMenuController : MonoBehaviour
{
    public Image loadingImage, circleImg;
    private bool allowSceneActivation = false;

    public TextMeshProUGUI pressE;


    public void PlayGame()
    {
        CacheObjects();
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

    IEnumerator LoadScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        loadingImage.gameObject.SetActive(true);
        // Знаходимо об'єкт Environment та робимо його неактивним
        GameObject parentEnvironment = GameObject.Find("Environment");
        parentEnvironment.SetActive(false);

        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {

            if (Input.GetKeyDown(KeyCode.E))
            {
                allowSceneActivation = true;
            }
            if (allowSceneActivation == true && asyncLoad.progress >= 0.9f)
            {
                circleImg.fillAmount = asyncLoad.progress;
                asyncLoad.allowSceneActivation = true;
                pressE.gameObject.SetActive(true);

            }

            yield return null;
        }
    }
}