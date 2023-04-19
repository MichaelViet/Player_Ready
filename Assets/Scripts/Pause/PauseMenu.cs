using UnityEngine;

public class PauseMenu : BasePauseMenu
{
    public PlayerMovement playerMovement;

    public override void Resume()
    {
<<<<<<< HEAD
<<<<<<< HEAD
        StartCoroutine(FadeOutMenu());
        StartCoroutine(FadeInAudioSources());
=======
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
>>>>>>> 225847647076aea25586628776fc6887ae55b500
=======
        StartCoroutine(FadeOutMenu());
        StartCoroutine(FadeInAudioSources());
>>>>>>> 7a4223b (Перехід між сценами, рефакторинг, анімації)
    }

    public override void Pause()
    {
<<<<<<< HEAD
<<<<<<< HEAD
        StartCoroutine(FadeInMenu());
        StartCoroutine(FadeOutAudioSources());
=======
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
>>>>>>> 225847647076aea25586628776fc6887ae55b500
=======
        StartCoroutine(FadeInMenu());
        StartCoroutine(FadeOutAudioSources());
>>>>>>> 7a4223b (Перехід між сценами, рефакторинг, анімації)
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
<<<<<<< HEAD
}
=======
}
>>>>>>> 225847647076aea25586628776fc6887ae55b500
=======
}
>>>>>>> 7a4223b (Перехід між сценами, рефакторинг, анімації)
