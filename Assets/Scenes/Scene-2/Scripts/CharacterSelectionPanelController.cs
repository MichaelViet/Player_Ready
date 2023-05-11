using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class CharacterSelectionPanelController : MonoBehaviour
{
    public CanvasGroup characterPanelCanvasGroup;
    public Image foxImage;
    public Image soldierImage;

    private float altHoldDuration = 0.2f;
    private float altHoldTime = 0f;
    private bool gamePaused = false;
    private BasePauseMenu isPaused;
    private PlayerSwitchController playerSwitchController;

    private void Start()
    {
        playerSwitchController = FindObjectOfType<PlayerSwitchController>();
        isPaused = FindObjectOfType<PauseMenu>();
        AddEventTrigger(foxImage, () => playerSwitchController.SwitchToFox());
        AddEventTrigger(soldierImage, () => playerSwitchController.SwitchToSoldaten());
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            isPaused.ToggleCursor(true);
            altHoldTime += Time.deltaTime;

            if (altHoldTime >= altHoldDuration)
            {
                StopCoroutine("FadeOut");
                StartCoroutine("FadeIn");
                if (!gamePaused)
                {
                    PauseGame();
                }
            }
        }
        else
        {
            altHoldTime = 0f;
            StopCoroutine("FadeIn");
            StartCoroutine("FadeOut");
            if (gamePaused)
            {
                ResumeGame();
            }
        }
    }

    private IEnumerator FadeIn()
    {
        while (characterPanelCanvasGroup.alpha < 1f)
        {
            characterPanelCanvasGroup.alpha += Time.unscaledDeltaTime;
            yield return null;
        }
        characterPanelCanvasGroup.alpha = 1f;
        characterPanelCanvasGroup.blocksRaycasts = true;
    }

    private IEnumerator FadeOut()
    {
        characterPanelCanvasGroup.blocksRaycasts = false;
        while (characterPanelCanvasGroup.alpha > 0f)
        {
            characterPanelCanvasGroup.alpha -= Time.unscaledDeltaTime;
            yield return null;
        }
        characterPanelCanvasGroup.alpha = 0f;
    }

    private void AddEventTrigger(Image image, UnityAction action)
    {
        EventTrigger eventTrigger = image.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) =>
        {
            action();
            playerSwitchController.ToggleCursor(image == soldierImage);
        });
        eventTrigger.triggers.Add(entry);
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        gamePaused = true;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        gamePaused = false;
    }
}
