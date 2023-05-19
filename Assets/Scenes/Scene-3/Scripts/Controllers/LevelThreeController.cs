using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelThreeController : MonoBehaviour
{
    public BasePauseMenu pauseMenu;
    public PlayerMotor playerMotor;
    int currentScene;
    SaveData data = new SaveData();
    private void Start()
    {
        pauseMenu = FindObjectOfType<BasePauseMenu>();
        pauseMenu.ToggleCursor(visible: true);
        // завантажте дані про гравця, якщо вони є
        if (SaveManager.IsGameSaved() && PlayerPrefs.HasKey("LoadedPlayerPositionX"))
        {
            data = SaveManager.LoadGame();
            float x = PlayerPrefs.GetFloat("LoadedPlayerPositionX");
            float y = PlayerPrefs.GetFloat("LoadedPlayerPositionY");
            float z = PlayerPrefs.GetFloat("LoadedPlayerPositionZ");
            playerMotor.transform.position = data.playerPosition;
            Vector3 loadedPosition = new Vector3(x, y, z);
            PlayerPrefs.DeleteKey("LoadedPlayerPositionX");
            PlayerPrefs.DeleteKey("LoadedPlayerPositionY");
            PlayerPrefs.DeleteKey("LoadedPlayerPositionZ");
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
        pauseMenu.PlaySaveAnimation();
        Debug.Log("Збереження гри...");

        // Зберегти позицію гравця
        data.playerPosition = playerMotor.transform.position;
        data.currentScene = SceneManager.GetActiveScene().buildIndex;

        PlayerPrefs.Save();
        SaveManager.SaveGame(data);
    }
}
