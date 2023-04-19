<<<<<<< HEAD
using System.Collections;
=======
>>>>>>> 225847647076aea25586628776fc6887ae55b500
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class BasePauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
<<<<<<< HEAD
    private CanvasGroup pauseMenuCanvasGroup;
    public AudioSource musicSource;
    public AudioSource soundSource;
    public AudioSource environmentSource;
    public static bool isPaused = false;
    public float fadeDuration = 0.5f;
    private float initialMusicVolume;
    private float initialSoundVolume;
    private float initialEnvironmentVolume;

    // зберігаємо початкове значення гучності музики, звуків та ефектів
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
=======

    public static bool isPaused = false;
>>>>>>> 225847647076aea25586628776fc6887ae55b500

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
<<<<<<< HEAD
        StartCoroutine(FadeOutMenu());
    }

    public virtual void Pause()
    {
        StartCoroutine(FadeInMenu());
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
=======
>>>>>>> 225847647076aea25586628776fc6887ae55b500
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

<<<<<<< HEAD
    // корутина для плавності зникнення аудіо
    protected IEnumerator FadeOutAudioSources()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            musicSource.volume = Mathf.Lerp(initialMusicVolume, 0, elapsedTime / fadeDuration);
            soundSource.volume = Mathf.Lerp(initialSoundVolume, 0, elapsedTime / fadeDuration);
            environmentSource.volume = Mathf.Lerp(initialEnvironmentVolume, 0, elapsedTime / fadeDuration);
            yield return null;
        }
        musicSource.Pause();
        soundSource.Pause();
        environmentSource.Pause();
    }

    // корутина для плавності появлення аудіо
    protected IEnumerator FadeInAudioSources()
    {
        musicSource.UnPause();
        soundSource.UnPause();
        environmentSource.UnPause();

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            musicSource.volume = Mathf.Lerp(0, initialMusicVolume, elapsedTime / fadeDuration);
            soundSource.volume = Mathf.Lerp(0, initialSoundVolume, elapsedTime / fadeDuration);
            environmentSource.volume = Mathf.Lerp(0, initialEnvironmentVolume, elapsedTime / fadeDuration);
            yield return null;
        }

=======
    public virtual void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
>>>>>>> 225847647076aea25586628776fc6887ae55b500
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
<<<<<<< HEAD
}
=======
}
>>>>>>> 225847647076aea25586628776fc6887ae55b500
