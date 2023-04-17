using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuController : BasePauseMenu, IPointerDownHandler
{
    public GameController gameController;
    public AudioSource musicSource;
    public AudioSource soundSource;
    public AudioSource environmentSource;
    public GameObject optionsPanel;

    public static bool ignoreMouseClick = false;

    private void OnEnable()
    {
        ignoreMouseClick = false;
    }

    public override void Resume()
    {
        base.Resume();
        gameController.enabled = true;
        musicSource.UnPause();
        soundSource.UnPause();
        environmentSource.UnPause();
    }

    public override void Pause()
    {
        base.Pause();
        gameController.enabled = false;
        musicSource.Pause();
        soundSource.Pause();
        environmentSource.Pause();
    }

    public void IgnoreMouseClick()
    {
        ignoreMouseClick = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        IgnoreMouseClick();
    }
}
