using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController2D controller;
    [SerializeField] private Animator animator;
    [SerializeField] private float runSpeed = 40f;

    private float horizontalMove = 0f;
    private bool jump = false;
    private bool crouch = false;
    private BasePauseMenu basePauseMenu;
    private Vector3 savedPosition;

    private void Start()
    {
        basePauseMenu = FindObjectOfType<BasePauseMenu>();
        if (SaveManager.IsGameSaved() && PlayerPrefs.HasKey("LoadedPlayerPositionX"))
        {
            float x = PlayerPrefs.GetFloat("LoadedPlayerPositionX");
            float y = PlayerPrefs.GetFloat("LoadedPlayerPositionY");
            float z = PlayerPrefs.GetFloat("LoadedPlayerPositionZ");
            Vector3 loadedPosition = new Vector3(x, y, z);
            SetPlayerPosition(loadedPosition);
            PlayerPrefs.DeleteKey("LoadedPlayerPositionX");
            PlayerPrefs.DeleteKey("LoadedPlayerPositionY");
            PlayerPrefs.DeleteKey("LoadedPlayerPositionZ");
        }
    }

    private void Update()
    {
        if (PauseMenu.isPaused) return;

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("IsJumping", true);
        }

        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }

        savedPosition = transform.position;
    }

    public void SetPlayerPosition(Vector3 position)
    {
        savedPosition = position;
        transform.position = position;
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    public void OnCrouching(bool isCrouching)
    {
        animator.SetBool("IsCrouching", isCrouching);
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

    public void SavePlayerProgress()
    {
        Debug.Log("Збереження гри...");
        basePauseMenu.PlaySaveAnimation();
        SaveData data = new SaveData();
        data.playerPosition = savedPosition;
        data.currentScene = SceneManager.GetActiveScene().buildIndex;

        SaveManager.SaveGame(data);
    }

    public void LoadPlayerProgress()
    {
        if (SaveManager.IsGameSaved())
        {
            SaveData data = SaveManager.LoadGame();
            SetPlayerPosition(data.playerPosition);
        }
    }
}
