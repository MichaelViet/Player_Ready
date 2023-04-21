using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class BasePauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private CanvasGroup pauseMenuCanvasGroup;
    public AudioSource musicSource;
    public AudioSource soundSource;
    public AudioSource environmentSource;
    public static bool isPaused = false;
    public float fadeDuration = 0.5f;
    public Image saveIcon;
    private float initialMusicVolume;
    private float initialSoundVolume;
    private float initialEnvironmentVolume;

    [SerializeField]
    public bool controlCursorVisibility = true;

    private void Awake()
    {
        initialMusicVolume = musicSource.volume;
        initialSoundVolume = soundSource.volume;
        initialEnvironmentVolume = environmentSource.volume;
    }

    protected virtual void Start()
    {
        pauseMenuCanvasGroup = pauseMenuUI.GetComponent<CanvasGroup>();
    }

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (Input.GetMouseButtonDown(1) && isPaused)
        {
            Resume();
        }
    }

    public virtual void Resume()
    {
        StartCoroutine(FadeOutMenu());
    }

    public virtual void Pause()
    {
        StartCoroutine(FadeInMenu());
    }
    public void ToggleCursor(bool visible)
    {
        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    protected IEnumerator FadeInMenu()
    {
        pauseMenuUI.SetActive(true);
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            pauseMenuCanvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            yield return null;
        }
        pauseMenuCanvasGroup.alpha = 1;
        Time.timeScale = 0f;
        isPaused = true;
        if (controlCursorVisibility)
        {
            ToggleCursor(true);
        }
    }

    protected IEnumerator FadeOutMenu()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            pauseMenuCanvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            yield return null;
        }
        pauseMenuCanvasGroup.alpha = 0;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        if (controlCursorVisibility)
        {
            ToggleCursor(false);
        }
    }

    protected IEnumerator FadeOutAudioSources()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;

            soundSource.volume = Mathf.Lerp(initialSoundVolume, 0, elapsedTime / fadeDuration);
            environmentSource.volume = Mathf.Lerp(initialEnvironmentVolume, 0, elapsedTime / fadeDuration);
            yield return null;
        }
        soundSource.Pause();
        environmentSource.Pause();
    }

    protected IEnumerator FadeInAudioSources()
    {
        soundSource.UnPause();
        environmentSource.UnPause();

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            soundSource.volume = Mathf.Lerp(0, initialSoundVolume, elapsedTime / fadeDuration);
            environmentSource.volume = Mathf.Lerp(0, initialEnvironmentVolume, elapsedTime / fadeDuration);
            yield return null;
        }
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        if (isPaused)
        {
            isPaused = false;
        }
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void PlaySaveAnimation()
    {
        StartCoroutine(SaveIconAnimation());
    }

    private IEnumerator SaveIconAnimation()
    {
        float fadeInDuration = 0.2f;
        float rotationDuration = 0.2f;
        float fadeOutDuration = 0.2f;

        float elapsedTime = 0f;
        saveIcon.gameObject.SetActive(true);

        // Поява картинки
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            saveIcon.color = new Color(saveIcon.color.r, saveIcon.color.g, saveIcon.color.b, Mathf.Lerp(0, 1, elapsedTime / fadeInDuration));
            yield return null;
        }
        saveIcon.color = new Color(saveIcon.color.r, saveIcon.color.g, saveIcon.color.b, 1);

        elapsedTime = 0f;

        // Обертання картинки
        float startRotation = saveIcon.rectTransform.eulerAngles.z;
        float targetRotation = startRotation + 360f;
        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float currentRotation = Mathf.Lerp(startRotation, targetRotation, elapsedTime / rotationDuration);
            saveIcon.rectTransform.eulerAngles = new Vector3(0, 0, currentRotation);
            yield return null;
        }
        saveIcon.rectTransform.eulerAngles = new Vector3(0, 0, targetRotation);

        elapsedTime = 0f;

        // Зникнення картинки
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            saveIcon.color = new Color(saveIcon.color.r, saveIcon.color.g, saveIcon.color.b, Mathf.Lerp(1, 0, elapsedTime / fadeOutDuration));
            yield return null;
        }
        saveIcon.color = new Color(saveIcon.color.r, saveIcon.color.g, saveIcon.color.b, 0);
        saveIcon.gameObject.SetActive(false);
    }
}