using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelThreeController : MonoBehaviour
{
    int currentScene;
    public BasePauseMenu pauseMenu;
    public PlayerMotor playerMotor;
    public CanvasGroup startScreenCanvasGroup;

    SaveData data = new SaveData();

    public void Start()
    {
        pauseMenu = FindObjectOfType<BasePauseMenu>();
        pauseMenu.ToggleCursor(visible: true);

        if (SaveManager.IsGameSaved() && PlayerPrefs.HasKey("PlayerPositionX") && PlayerPrefs.HasKey("PlayerPositionY") && PlayerPrefs.HasKey("PlayerPositionZ"))
        {
            Debug.Log("Завантаження гри...");
            data = SaveManager.LoadGame();
            if (playerMotor != null)
            {
                playerMotor.SetPosition(data.playerMotorPosition);
                PlayerPrefs.DeleteKey("PlayerPositionX");
                PlayerPrefs.DeleteKey("PlayerPositionY");
                PlayerPrefs.DeleteKey("PlayerPositionZ");
            }
        }
    }

    private void Update()
    {
        if (PauseMenu.isPaused)
        {
            return;
        }
    }

    public void SavePlayerProgress()
    {
        Debug.Log("Збереження гри...");
        pauseMenu.PlaySaveAnimation();
        data.playerMotorPosition = playerMotor.transform.position;
        data.currentScene = SceneManager.GetActiveScene().buildIndex;

        SaveManager.SaveGame(data);
    }
}

