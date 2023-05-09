using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public GameObject foxPlayer;
    public GameObject soldatenPlayer;
    public GameObject currentPlayer;
    public CinemachineVirtualCamera virtualCamera;
    private BasePauseMenu pauseMenu;
    void Start()
    {
        currentPlayer = foxPlayer;
        soldatenPlayer.SetActive(false);
        pauseMenu = FindObjectOfType<BasePauseMenu>();
    }

    void Update()
    {
        SyncCharacterTransforms();
    }

    public void SwitchToFox()
    {
        currentPlayer = foxPlayer;
        foxPlayer.SetActive(true);
        soldatenPlayer.SetActive(false);
    }
    public void ToggleCursor(bool showCursor)
    {
        if (pauseMenu != null)
        {
            pauseMenu.ToggleCursor(showCursor);
        }
    }
    public void SwitchToSoldaten()
    {
        currentPlayer = soldatenPlayer;
        soldatenPlayer.SetActive(true);
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
}
