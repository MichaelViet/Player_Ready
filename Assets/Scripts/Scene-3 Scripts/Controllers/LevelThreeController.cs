using System.Collections;
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
        if (SaveManager.IsGameSaved() && PlayerPrefs.HasKey("PlayerPositionX"))
        {
            data = SaveManager.LoadGame();
            playerMotor.transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerPositionX"), PlayerPrefs.GetFloat("PlayerPositionY"), PlayerPrefs.GetFloat("PlayerPositionZ"));
        }

        StartCoroutine(WaitForAlphaThenLoad());
    }

    private IEnumerator WaitForAlphaThenLoad()
    {
        // Чекаємо поки альфа стане 0.9
        while (startScreenCanvasGroup.alpha < 0.9f)
        {
            yield return null;
        }

        LoadPlayerProgress();

        // Запускаємо анімацію зникнення
        float duration = 3f; // тривалість анімації в секундах
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            startScreenCanvasGroup.alpha = Mathf.Lerp(0.9f, 0f, (Time.time - startTime) / duration);
            yield return null;
        }

        // Після анімації вимкнемо CanvasGroup
        startScreenCanvasGroup.alpha = 0f;
        startScreenCanvasGroup.gameObject.SetActive(false);
    }


    private void Update()
    {
        if (PauseMenu.isPaused)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            LoadPlayerProgress();
        }
    }

    public void LoadPlayerProgress()
    {
        data = SaveManager.LoadGame();
        if (playerMotor != null)
        {
            playerMotor.transform.position = data.playerMotor;
        }
        Debug.Log("Завантаження гри...");
    }

    public void SavePlayerProgress()
    {
        pauseMenu.PlaySaveAnimation();
        Debug.Log("Збереження гри...");
        PlayerPrefs.SetFloat("PlayerPositionX", data.playerMotor.x);
        PlayerPrefs.SetFloat("PlayerPositionY", data.playerMotor.y);
        PlayerPrefs.SetFloat("PlayerPositionZ", data.playerMotor.z);
        PlayerPrefs.Save();

        data.currentScene = SceneManager.GetActiveScene().buildIndex;
        data.playerMotor = playerMotor.transform.position;
        SaveManager.SaveGame(data);
    }
}

