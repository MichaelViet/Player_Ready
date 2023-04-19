<<<<<<< HEAD
<<<<<<< HEAD
using System.Collections;
=======
>>>>>>> 225847647076aea25586628776fc6887ae55b500
=======
using System.Collections;
>>>>>>> 7a4223b (Перехід між сценами, рефакторинг, анімації)
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuController : BasePauseMenu, IPointerDownHandler
{
    public GameController gameController;
<<<<<<< HEAD
<<<<<<< HEAD
    public GameObject optionsPanel;
=======
    public AudioSource musicSource;
    public AudioSource soundSource;
    public AudioSource environmentSource;
    public GameObject optionsPanel;

>>>>>>> 225847647076aea25586628776fc6887ae55b500
=======
    public GameObject optionsPanel;
>>>>>>> 7a4223b (Перехід між сценами, рефакторинг, анімації)
    public static bool ignoreMouseClick = false;

    private void OnEnable()
    {
        ignoreMouseClick = false;
    }

    public override void Resume()
    {
<<<<<<< HEAD
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
        base.Pause();
        gameController.enabled = false;
        musicSource.Pause();
        soundSource.Pause();
        environmentSource.Pause();
>>>>>>> 225847647076aea25586628776fc6887ae55b500
=======
        StartCoroutine(FadeInMenu());
        StartCoroutine(FadeOutAudioSources());
>>>>>>> 7a4223b (Перехід між сценами, рефакторинг, анімації)
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
