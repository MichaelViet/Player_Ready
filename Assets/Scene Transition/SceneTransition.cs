using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneTransition : MonoBehaviour
{
    public TextMeshProUGUI LoadingPercentage;
    public Image LoadingProgressBar;

    private static SceneTransition instance;
    private static bool shouldPlayOpeningAnimation = false;
    private static string sceneName;

    private Animator componentAnimator;
    private AsyncOperation loadingSceneOperation;

    public static void SwitchToScene(string _sceneName)
    {
        sceneName = _sceneName;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            instance.componentAnimator.SetTrigger("sceneEnding");

            instance.loadingSceneOperation = SceneManager.LoadSceneAsync(sceneName);

            instance.loadingSceneOperation.allowSceneActivation = false;

            instance.LoadingProgressBar.fillAmount = 0;
        }

        if (loadingSceneOperation != null)
        {
            LoadingPercentage.text = Mathf.RoundToInt(loadingSceneOperation.progress * 100) + "%";

            LoadingProgressBar.fillAmount = Mathf.Lerp(LoadingProgressBar.fillAmount, loadingSceneOperation.progress,
                Time.deltaTime * 5);
        }
    }

    private void Start()
    {
        instance = this;

        componentAnimator = GetComponent<Animator>();

        if (shouldPlayOpeningAnimation)
        {
            componentAnimator.SetTrigger("sceneOpening");
            instance.LoadingProgressBar.fillAmount = 1;

            shouldPlayOpeningAnimation = false;
        }
    }

    public void OnAnimationOver()
    {
        shouldPlayOpeningAnimation = true;

        loadingSceneOperation.allowSceneActivation = true;
    }
}
