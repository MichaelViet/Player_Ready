using UnityEngine;

public class EmptyPlayer : MonoBehaviour
{
    [SerializeField] private CharacterController2D controller;
    [SerializeField] private float runSpeed = 40f;
    private DialogReader dialogReader;
    private float horizontalMove = 0f;
    private bool jump = false;
    private bool crouch = false;
    public CanvasGroup monologPanel;
    private bool canMove = true;
    void Start()
    {
        dialogReader = FindObjectOfType<DialogReader>();
    }

    private void Update()
    {
        if (PauseMenu.isPaused)
        {
            return;
        }

        if (MonologueZone.currentZone != null)
        {
            canMove = !MonologueZone.currentZone.playerStop;
        }
        else
        {
            canMove = true;
        }
        horizontalMove = canMove ? Input.GetAxisRaw("Horizontal") * runSpeed : 0;

        if (canMove && Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        }

        else if (canMove && Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }
    }

    private void FixedUpdate()
    {

        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }
}
