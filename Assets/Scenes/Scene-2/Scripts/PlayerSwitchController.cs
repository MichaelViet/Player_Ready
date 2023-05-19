using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class PlayerSwitchController : MonoBehaviour
{
    public GameObject foxPlayer;
    public GameObject soldatenPlayer;
    public GameObject currentPlayer;
    public CinemachineVirtualCamera virtualCamera;
    private BasePauseMenu pauseMenu;
    private InventoryUIController inventoryController;
    private Player foxPlayerScript;
    private Player soldatenPlayerScript;
    void Start()
    {
        currentPlayer = foxPlayer;
        soldatenPlayer.SetActive(false);
        pauseMenu = FindObjectOfType<BasePauseMenu>();
        inventoryController = FindObjectOfType<InventoryUIController>();
        foxPlayerScript = foxPlayer.GetComponent<Player>();
        soldatenPlayerScript = soldatenPlayer.GetComponent<Player>();
    }

    void Update()
    {
        SyncCharacterTransforms();
        HandleCursorVisibility();
    }
    public void ToggleCursor(bool showCursor)
    {
        if (pauseMenu != null)
        {
            pauseMenu.ToggleCursor(showCursor);
        }
    }

    public void SwitchToFox()
    {
        currentPlayer = foxPlayer;
        foxPlayer.SetActive(true);
        foxPlayerScript.StartCoroutine(foxPlayerScript.RegenHealth());
        int tempHealth = soldatenPlayerScript.health; // Зберігаємо поточне здоров'я солдата
        foxPlayerScript.health = tempHealth; // Встановлюємо здоров'я лисиці таким же, як у солдата
        foxPlayerScript.healthSlider.maxValue = 250; // Встановлюємо максимальне значення слайдера
        foxPlayerScript.healthSlider.value = tempHealth; // Оновлюємо значення слайдера здоров'я
        soldatenPlayer.SetActive(false);
    }

    public void SwitchToSoldaten()
    {
        currentPlayer = soldatenPlayer;
        soldatenPlayer.SetActive(true);
        soldatenPlayerScript.StartCoroutine(soldatenPlayerScript.RegenHealth());
        int tempHealth = foxPlayerScript.health; // Зберігаємо поточне здоров'я лисиці
        soldatenPlayerScript.health = tempHealth; // Встановлюємо здоров'я солдата таким же, як у лисиці
        soldatenPlayerScript.healthSlider.maxValue = 250; // Встановлюємо максимальне значення слайдера
        soldatenPlayerScript.healthSlider.value = tempHealth; // Оновлюємо значення слайдера здоров'я
        foxPlayer.SetActive(false);
        pauseMenu.ToggleCursor(true);
    }

    void ResetAnimator(Animator animator)
    {
        animator.Play("Idle", 0, 0f);
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsCrouching", false);
    }

    void SyncCharacterTransforms()
    {
        if (currentPlayer == foxPlayer)
        {
            soldatenPlayer.transform.position = currentPlayer.transform.position;
        }
        else
        {
            foxPlayer.transform.position = currentPlayer.transform.position;
        }
    }

    void HandleCursorVisibility()
    {
        if (BasePauseMenu.isPaused)
        {
            ToggleCursor(true);
        }
        else
        {
            if (currentPlayer == foxPlayer)
            {
                ToggleCursor(false);
            }
            else
            {
                ToggleCursor(true);
            }
        }
        if (inventoryController.inventoryPanel.alpha > 0)
        {
            ToggleCursor(true);
        }
    }
}
