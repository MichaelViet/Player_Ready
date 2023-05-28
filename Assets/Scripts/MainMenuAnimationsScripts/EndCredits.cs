using System.Collections;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndCredits : MonoBehaviour
{
    public float scrollSpeed = 30f;
    public string fileName = "end_credits.txt";
    public string mainMenuScene = "MainMenu";
    public Image fadeOutImage;
    private TextMeshProUGUI creditsText;
    private RectTransform rectTransform;

    void Start()
    {
        SaveManager.ClearSavedGame();
        creditsText = GetComponent<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            creditsText.text = dataAsJson;
        }
    }

    void Update()
    {
        rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);

        if (creditsText.preferredHeight < rectTransform.anchoredPosition.y)
        {
            StartCoroutine(LoadSceneWithFadeOut(mainMenuScene, 10f)); // Завантаження головного меню з плавним зниканням
        }
    }

    IEnumerator LoadSceneWithFadeOut(string sceneName, float transitionTime)
    {
        float t = 0;
        while (t < transitionTime)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, t / transitionTime);
            fadeOutImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}
