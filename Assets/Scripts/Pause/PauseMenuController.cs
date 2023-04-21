using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuController : BasePauseMenu, IPointerDownHandler
{
    public GameController gameController;
    public static bool ignoreMouseClick = false;

    private void OnEnable()
    {
        ignoreMouseClick = false;
    }

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

    public void IgnoreMouseClick()
    {
        ignoreMouseClick = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        IgnoreMouseClick();
    }
}
