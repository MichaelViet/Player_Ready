using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChooseController : MonoBehaviour
{
    public ChooseLabelController label;
    public GameController gameController;
    private RectTransform rectTransform;
    private Animator animator;
    private float labelHeight = -1;
    public GameObject fadeImage; // Reference to the FadeImage GameObject
    private CanvasGroup fadeImageCanvasGroup;
    void Start()
    {
        animator = GetComponent<Animator>();
        rectTransform = GetComponent<RectTransform>();
        fadeImageCanvasGroup = fadeImage.GetComponent<CanvasGroup>(); // Get the CanvasGroup component
    }

    public void SetupChoose(ChooseScene scene)
    {
        DestroyLabels();
        animator.SetTrigger("Show");
        for (int index = 0; index < scene.labels.Count; index++)
        {
            ChooseLabelController newLabel = Instantiate(label.gameObject, transform).GetComponent<ChooseLabelController>();

            if (labelHeight == -1)
            {
                labelHeight = newLabel.GetHeight();
            }

            newLabel.Setup(scene.labels[index], this, CalculateLabelPosition(index, scene.labels.Count));
        }

        Vector2 size = rectTransform.sizeDelta;
        size.y = (scene.labels.Count + 2) * labelHeight;
        rectTransform.sizeDelta = size;
    }

    public void PerformChoose(ChooseScene.ChooseLabel label)
    {
        if (label.nextScene != null)
        {
            gameController.PlayScene(label.nextScene);
        }
        else if (!string.IsNullOrEmpty(label.nextSceneName))
        {
            StartCoroutine(FadeInAndLoadScene(label.nextSceneName));
        }
        animator.SetTrigger("Hide");
    }

    private IEnumerator FadeInAndLoadScene(string sceneName)
    {
        float fadeDuration = 3.0f; // You can adjust the fade duration to your preference
        float elapsedTime = 0.0f;
        float startAlpha = fadeImageCanvasGroup.alpha;

        // Enable the FadeImage GameObject before starting the fade-in animation
        fadeImage.SetActive(true);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeImageCanvasGroup.alpha = Mathf.Lerp(startAlpha, 1, elapsedTime / fadeDuration);
            yield return null;
        }

        fadeImageCanvasGroup.alpha = 1;

        // Load the new scene after the fade-in animation is complete
        SceneManager.LoadScene(sceneName);
    }

    private float CalculateLabelPosition(int labelIndex, int labelCount)
    {
        if (labelCount % 2 == 0)
        {
            if (labelIndex < labelCount / 2)
            {
                return labelHeight * (labelCount / 2 - labelIndex - 1) + labelHeight / 2;
            }
            else
            {
                return -1 * (labelHeight * (labelIndex - labelCount / 2) + labelHeight / 2);
            }
        }
        else
        {
            if (labelIndex < labelCount / 2)
            {
                return labelHeight * (labelCount / 2 - labelIndex);
            }
            else if (labelIndex > labelCount / 2)
            {
                return -1 * (labelHeight * (labelIndex - labelCount / 2));
            }
            else
            {
                return 0;
            }
        }
    }

    private void DestroyLabels()
    {
        foreach (Transform childTransform in transform)
        {
            Destroy(childTransform.gameObject);
        }
    }
}
