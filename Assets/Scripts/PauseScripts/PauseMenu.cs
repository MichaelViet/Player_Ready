using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : BasePauseMenu
{
    public LevelManager levelManager;
    public LevelThreeController levelThreeController;

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
    public void SavePlayerProgressLevelThree()
    {
        levelThreeController.SavePlayerProgress();
        Debug.Log("Збереження гри відбулося");
    }
}