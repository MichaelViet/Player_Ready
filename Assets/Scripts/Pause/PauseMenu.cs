using UnityEngine;

public class PauseMenu : BasePauseMenu
{
    public LevelManager levelManager;

    public override void Resume()
    {
        StartCoroutine(FadeOutMenu());
        StartCoroutine(FadeInAudioSources());
    }

    public override void Pause()
    {
        StartCoroutine(FadeInMenu());
        StartCoroutine(FadeOutAudioSources());
    }

    public void SavePlayerProgress()
    {
        levelManager.SavePlayerProgress();
    }
}