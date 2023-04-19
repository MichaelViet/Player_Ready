<<<<<<< HEAD
using System.Collections;
=======
>>>>>>> 225847647076aea25586628776fc6887ae55b500
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuController : BasePauseMenu, IPointerDownHandler
{
    public GameController gameController;
<<<<<<< HEAD
    public GameObject optionsPanel;
=======
    public AudioSource musicSource;
    public AudioSource soundSource;
    public AudioSource environmentSource;
    public GameObject optionsPanel;

>>>>>>> 225847647076aea25586628776fc6887ae55b500
    public static bool ignoreMouseClick = false;

    private void OnEnable()
    {
        ignoreMouseClick = false;
    }

    public override void Resume()
    {
<<<<<<< HEAD
        StartCoroutine(FadeOutMenu());
        StartCoroutine(FadeInAudioSources());
=======
        base.Resume();
        gameController.enabled = true;
        musicSource.UnPause();
        soundSource.UnPause();
        environmentSource.UnPause();
>>>>>>> 225847647076aea25586628776fc6887ae55b500
    }

    public override void Pause()
    {
<<<<<<< HEAD
        StartCoroutine(FadeInMenu());
        StartCoroutine(FadeOutAudioSources());
=======
        base.Pause();
        gameController.enabled = false;
        musicSource.Pause();
        soundSource.Pause();
        environmentSource.Pause();
>>>>>>> 225847647076aea25586628776fc6887ae55b500
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
