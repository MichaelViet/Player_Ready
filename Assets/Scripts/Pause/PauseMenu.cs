using UnityEngine;

public class PauseMenu : BasePauseMenu
{
    public PlayerMovement playerMovement;

    public override void Resume()
    {
<<<<<<< HEAD
        StartCoroutine(FadeOutMenu());
        StartCoroutine(FadeInAudioSources());
=======
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
>>>>>>> 225847647076aea25586628776fc6887ae55b500
    }

    public override void Pause()
    {
<<<<<<< HEAD
        StartCoroutine(FadeInMenu());
        StartCoroutine(FadeOutAudioSources());
=======
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
>>>>>>> 225847647076aea25586628776fc6887ae55b500
    }

    public void SavePlayerProgress()
    {
        playerMovement.SavePlayerProgress();
    }

    public void LoadPlayerProgress()
    {
        playerMovement.LoadPlayerProgress();
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> 225847647076aea25586628776fc6887ae55b500
