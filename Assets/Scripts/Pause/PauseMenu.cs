using UnityEngine;

public class PauseMenu : BasePauseMenu
{
    public PlayerMovement playerMovement;

    public override void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public override void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void SavePlayerProgress()
    {
        playerMovement.SavePlayerProgress();
    }

    public void LoadPlayerProgress()
    {
        playerMovement.LoadPlayerProgress();
    }
}
