using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameController gameController;

    public AudioSource musicSource;
    public AudioSource soundSource;
    public AudioSource environmentSource;

    public GameObject optionsPanel;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
            if (optionsPanel.activeSelf)
            {
                Pause();
            }

        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameController.enabled = true;
        GameIsPaused = false;

        musicSource.pitch = 1f;
        soundSource.pitch = 1f;
        environmentSource.pitch = 1f;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameController.enabled = false;
        GameIsPaused = true;

        //musicSource.pitch *= 0f;
        soundSource.pitch *= 0f;
        environmentSource.pitch *= 0f;

    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}