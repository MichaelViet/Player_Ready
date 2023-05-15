using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public AudioClip levelMusic; // Музика для рівня
    public AudioClip levelAmbience; // Звук оточення для рівня
    public BasePauseMenu pauseMenu;
    private AudioController audioController;
    public CanvasGroup canvasGroup;
    public float fadeDuration = 3.0f;
    public float musicFadeDuration = 2.0f;
    public Player player;
    public DialogReader dialogReader;

    SaveData data = new SaveData();
    public void Start()
    {
        player = FindObjectOfType<Player>();
        dialogReader = FindObjectOfType<DialogReader>();
        pauseMenu.ToggleCursor(false);
        WizardController wizard = FindObjectOfType<WizardController>();
        StartCoroutine(PlayAnimationAndDeactivateCanvas());
        if (SaveManager.IsGameSaved() && PlayerPrefs.HasKey("LoadedPlayerPositionX"))
        {   // Загрузка гравця
            data = SaveManager.LoadGame();
            float x = PlayerPrefs.GetFloat("LoadedPlayerPositionX");
            float y = PlayerPrefs.GetFloat("LoadedPlayerPositionY");
            float z = PlayerPrefs.GetFloat("LoadedPlayerPositionZ");
            int loadedCurrentDialogIndex = PlayerPrefs.GetInt("LoadedCurrentDialogIndex");
            int loadedCurrentSentenceIndex = PlayerPrefs.GetInt("LoadedCurrentSentenceIndex");
            Vector3 loadedPosition = new Vector3(x, y, z);
            player.transform.position = loadedPosition;
            PlayerPrefs.DeleteKey("LoadedPlayerPositionX");
            PlayerPrefs.DeleteKey("LoadedPlayerPositionY");
            PlayerPrefs.DeleteKey("LoadedPlayerPositionZ");

            dialogReader.SetCurrentDialogIndex(loadedCurrentDialogIndex);
            dialogReader.SetCurrentSentenceIndex(loadedCurrentSentenceIndex);
            PlayerPrefs.DeleteKey("LoadedCurrentDialogIndex");
            PlayerPrefs.DeleteKey("LoadedCurrentSentenceIndex");
            if (wizard != null)
            {
                float wizardX = PlayerPrefs.GetFloat("LoadedWizardPositionX");
                float wizardY = PlayerPrefs.GetFloat("LoadedWizardPositionY");
                float wizardZ = PlayerPrefs.GetFloat("LoadedWizardPositionZ");
                Vector3 loadedWizardPosition = new Vector3(wizardX, wizardY, wizardZ);
                wizard.transform.position = loadedWizardPosition;
                wizard.Wall.SetActive(data.wallActive);
                wizard.dialogComplete = data.dialogCompleted; // відновлюємо стан діалогу

                PlayerPrefs.DeleteKey("LoadedWizardPositionX");
                PlayerPrefs.DeleteKey("LoadedWizardPositionY");
                PlayerPrefs.DeleteKey("LoadedWizardPositionZ");
            }
            else
            {
                Debug.LogError("Не знайдено компонента WizardController.");
            }
            // Загрузка дерева
            TreeDestruction treeDestruction = FindObjectOfType<TreeDestruction>();
            if (treeDestruction != null)
            {
                treeDestruction.transform.position = data.treePosition;
                treeDestruction.transform.rotation = data.treeRotation;
                treeDestruction.SetIsDestroyed(data.isTreeDestroyed);
                treeDestruction.SetEmptyWallActive(data.emptyWallActive);
                treeDestruction.InteractionRadius = data.InteractionRadius;
                wizard.dialogComplete = data.dialogCompleted; // відновлюємо стан діалогу
                if (wizard.dialogComplete)
                {
                    int lastIndex = dialogReader.dialogData.dialog.Count - 1;
                    if (lastIndex >= 0)
                    {
                        DialogEntry lastDialogEntry = dialogReader.dialogData.dialog[lastIndex];
                        dialogReader.bottomBarText.text = lastDialogEntry.sentences[lastDialogEntry.sentences.Count - 1];
                        dialogReader.personNameText.text = lastDialogEntry.speaker;
                    }
                }
                PlayerPrefs.DeleteKey("LoadedWizardPositionX");
                PlayerPrefs.DeleteKey("LoadedWizardPositionY");
                PlayerPrefs.DeleteKey("LoadedWizardPositionZ");
            }
            else
            {
                Debug.LogError("Не знайдено компонента TreeDestruction.");
            }
            // Завантаження радіусів
            MonologueZone[] monologueZones = FindObjectsOfType<MonologueZone>();
            for (int i = 0; i < monologueZones.Length; i++)
            {
                string key = "Zone" + monologueZones[i].zoneIndex + "Radius";
                if (PlayerPrefs.HasKey(key))
                {
                    monologueZones[i].radius = PlayerPrefs.GetFloat(key);
                }
            }
            // Завантаження квесту
            QuestSystem questSystem = FindObjectOfType<QuestSystem>();
            if (questSystem != null)
            {
                for (int i = 0; i < data.questStates.Count; i++)
                {
                    questSystem.questList[i].IsComplete = data.questStates[i].IsComplete;
                    if (!questSystem.questList[i].IsComplete)
                    {
                        questSystem.questList[i].IsActive = data.questStates[i].IsActive;
                    }
                    else
                    {
                        questSystem.questList[i].IsActive = false;
                    }
                }
            }
            else
            {
                Debug.LogError("QuestSystem not found.");
            }

        }
    }

    public void SavePlayerProgress()
    {
        Debug.Log("Збереження гри...");
        pauseMenu.PlaySaveAnimation();
        data.playerPosition = player.transform.position;
        data.currentScene = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("LoadedCurrentDialogIndex", dialogReader.GetCurrentDialogIndex());
        PlayerPrefs.SetInt("LoadedCurrentSentenceIndex", dialogReader.GetCurrentSentenceIndex());
        PlayerPrefs.Save();
        Debug.Log("Saving the game...");

        // Збереження позиції чаклуна
        WizardController wizard = FindObjectOfType<WizardController>();
        if (wizard != null)
        {
            data.wizardPosition = wizard.transform.position;
            data.wallActive = wizard.Wall.activeSelf;
            data.dialogCompleted = wizard.dialogComplete;
        }
        else
        {
            Debug.LogError("Не знайдено компонента WizardController.");
        }

        if (dialogReader != null)
        {
            data.currentDialogIndex = dialogReader.GetCurrentDialogIndex();
            data.currentSentenceIndex = dialogReader.GetCurrentSentenceIndex();
        }
        else
        {
            Debug.LogError("Не знайдено компонента DialogReader.");
        }
        // Зберігання стану дерева
        TreeDestruction treeDestruction = FindObjectOfType<TreeDestruction>();
        if (treeDestruction != null)
        {
            data.treePosition = treeDestruction.transform.position;
            data.treeRotation = treeDestruction.transform.rotation;
            data.isTreeDestroyed = treeDestruction.IsDestroyed;
            data.emptyWallActive = treeDestruction.emptyWallActive;
            data.InteractionRadius = treeDestruction.InteractionRadius;
        }
        else
        {
            Debug.LogError("Не знайдено компонента TreeDestruction.");
        }
        // Збереження радіусів
        MonologueZone[] monologueZones = FindObjectsOfType<MonologueZone>();
        for (int i = 0; i < monologueZones.Length; i++)
        {
            PlayerPrefs.SetFloat("Zone" + monologueZones[i].zoneIndex + "Radius", monologueZones[i].radius);
        }
        // Збереження квестів
        QuestSystem questSystem = FindObjectOfType<QuestSystem>();
        if (questSystem != null)
        {
            data.questStates = new List<Quest>();
            foreach (var quest in questSystem.questList)
            {
                data.questStates.Add(new Quest { IsActive = quest.IsActive, IsComplete = quest.IsComplete });
            }
        }
        else
        {
            Debug.LogError("QuestSystem not found.");
        }
        PlayerPrefs.Save();
        Debug.Log("Saving the game...");
        SaveManager.SaveGame(data);
    }

    private IEnumerator PlayAnimationAndDeactivateCanvas()
    {

        yield return StartCoroutine(FadeInLevel());

        canvasGroup.gameObject.SetActive(false);

        audioController = FindObjectOfType<AudioController>();

        if (audioController != null)
        {
            audioController.PlayAudio(levelMusic, null, levelAmbience);
        }
        else
        {
            Debug.LogWarning("Не знайдено компонента AudioController.");
        }
    }
    private IEnumerator FadeInLevel()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            {
                elapsedTime += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
                yield return null;
            }
            canvasGroup.alpha = 0f;
        }

    }
}