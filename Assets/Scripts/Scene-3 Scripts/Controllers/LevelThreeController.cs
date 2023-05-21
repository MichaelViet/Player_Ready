using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelThreeController : MonoBehaviour
{
    int currentScene;
    public BasePauseMenu pauseMenu;
    public PlayerMotor playerMotor;
    public CanvasGroup startScreenCanvasGroup;
    public DialogReader dialogReader;
    private HintManager hintManager;
    public QuestSystem questSystem;
    private bool questActivated = false;
    public bool fadeInCalled = false;
    private PlayerController playerController;
    private bool hintShown = false; // Відслідковує, чи був показаний підказка
    SaveData data = new SaveData();

    public void Start()
    {
        pauseMenu = FindObjectOfType<BasePauseMenu>();
        playerMotor = FindObjectOfType<PlayerMotor>();
        dialogReader = FindObjectOfType<DialogReader>();
        hintManager = FindObjectOfType<HintManager>();
        questSystem = FindObjectOfType<QuestSystem>();
        playerController = FindObjectOfType<PlayerController>();
        pauseMenu.ToggleCursor(visible: true);
        questActivated = false;
        LoadPlayerProgress();
    }

    private void Update()
    {
        if (PauseMenu.isPaused)
        {
            return;
        }
        if (dialogReader.currentDialogIndex >= dialogReader.dialogData.dialog.Count && !fadeInCalled)
        {
            questSystem.StartCoroutine(questSystem.FadeIn());
            fadeInCalled = true;
            hintManager.ShowHint(0); // Викликаємо ShowHint з індексом 0
            hintShown = true; // Оновлюємо, що підказка була показана
        }
    }

    public void LoadPlayerProgress()
    {
        if (SaveManager.IsGameSaved() && PlayerPrefs.HasKey("PlayerPositionX") && PlayerPrefs.HasKey("PlayerPositionY") && PlayerPrefs.HasKey("PlayerPositionZ"))
        {
            data = SaveManager.LoadGame();

            // Загрузка стану діалога
            if (dialogReader != null && !playerController.dialogComplete)
            {
                int loadedCurrentDialogIndex = PlayerPrefs.GetInt("LoadedCurrentDialogIndex");
                int loadedCurrentSentenceIndex = PlayerPrefs.GetInt("LoadedCurrentSentenceIndex");
                dialogReader.SetCurrentDialogIndex(loadedCurrentDialogIndex);
                dialogReader.SetCurrentSentenceIndex(loadedCurrentSentenceIndex);
                PlayerPrefs.DeleteKey("LoadedCurrentDialogIndex");
                PlayerPrefs.DeleteKey("LoadedCurrentSentenceIndex");
            }

            if (PlayerPrefs.HasKey("DialogComplete"))
            {
                bool dialogComplete = PlayerPrefs.GetInt("DialogComplete") == 1 ? true : false;
                dialogReader.bottomBarCanvasGroup.alpha = dialogComplete ? 0f : 1f; // Показуйте bottomBar, якщо діалог не завершено
            }

            // Загрузка гравця
            if (playerMotor != null)
            {
                playerMotor.SetPosition(data.playerMotorPosition);
                playerController.dialogComplete = data.dialogCompleted;
                PlayerPrefs.DeleteKey("PlayerPositionX");
                PlayerPrefs.DeleteKey("PlayerPositionY");
                PlayerPrefs.DeleteKey("PlayerPositionZ");
                fadeInCalled = data.fadeInCalled;
            }

            // Завантаження квесту
            if (questSystem != null)
            {
                for (int i = 0; i < data.questStates.Count; i++)
                {
                    questSystem.questList[i].IsActive = data.questStates[i].IsActive;
                    questSystem.questList[i].IsComplete = data.questStates[i].IsComplete;
                }
            }
            // Завантаження стану підказок
            HintManager hintManager = FindObjectOfType<HintManager>();
            if (hintManager != null)
            {
                for (int i = 0; i < hintManager.hints.Count; i++)
                {
                    string key = "Hint" + i;
                    if (PlayerPrefs.HasKey(key))
                    {
                        hintManager.hints[i].hasBeenShown = PlayerPrefs.GetInt(key) == 1;
                    }
                }
            }
        }
    }
    public void SavePlayerProgress()
    {
        Debug.Log("Збереження гри...");
        pauseMenu.PlaySaveAnimation();
        // Збереження сцени
        data.currentScene = SceneManager.GetActiveScene().buildIndex;

        // Збереження гравця
        data.playerMotorPosition = playerMotor.transform.position;
        data.dialogCompleted = playerController.dialogComplete;

        // Збереження діалогу
        if (dialogReader != null)
        {
            data.currentDialogIndex = dialogReader.GetCurrentDialogIndex();
            data.currentSentenceIndex = dialogReader.GetCurrentSentenceIndex();
            PlayerPrefs.SetInt("LoadedCurrentDialogIndex", data.currentDialogIndex);
            PlayerPrefs.SetInt("LoadedCurrentSentenceIndex", data.currentSentenceIndex);
        }

        // Збереження стану квестів
        data.questStates = new List<Quest>();
        foreach (var quest in questSystem.questList)
        {
            data.questStates.Add(new Quest { IsActive = quest.IsActive, IsComplete = quest.IsComplete });
            data.fadeInCalled = fadeInCalled = true;
        }

        // Збереження стану підказок
        if (hintManager != null)
        {
            for (int i = 0; i < hintManager.hints.Count; i++)
            {
                PlayerPrefs.SetInt("Hint" + i, hintManager.hints[i].hasBeenShown ? 1 : 0);
            }
        }
        SaveManager.SaveGame(data);
    }
}

