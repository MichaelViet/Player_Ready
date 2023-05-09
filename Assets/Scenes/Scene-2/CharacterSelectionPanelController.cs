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
    private PlayerController playerController;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        isPaused = FindObjectOfType<PauseMenu>();
        AddEventTrigger(foxImage, () => playerController.SwitchToFox());
        AddEventTrigger(soldierImage, () => playerController.SwitchToSoldaten());
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            isPaused.ToggleCursor(true);
            altHoldTime += Time.deltaTime;

            if (altHoldTime >= altHoldDuration)
            {
                characterPanelCanvasGroup.alpha = 1f;
                characterPanelCanvasGroup.blocksRaycasts = true;
                if (!gamePaused)
                {
                    PauseGame();
                }
            }
        }
        else
        {
            altHoldTime = 0f;
            characterPanelCanvasGroup.alpha = 0f;
            characterPanelCanvasGroup.blocksRaycasts = false;
            if (gamePaused)
            {
                ResumeGame();
            }
        }
    }

    private void AddEventTrigger(Image image, UnityAction action)
    {
        EventTrigger eventTrigger = image.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) =>
        {
            action();
            playerController.ToggleCursor(image == soldierImage);
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
