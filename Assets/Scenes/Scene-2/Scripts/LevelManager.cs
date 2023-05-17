using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public float fadeDuration = 3.0f;
    public float musicFadeDuration = 2.0f;
    public AudioClip levelMusic; // Музика для рівня
    public BasePauseMenu pauseMenu;
    private AudioController audioController;
    public CanvasGroup StartCanvasGroup;
    public Player player;
    public DialogReader dialogReader;
    public Boss boss;
    public PowerStone powerStone;
    public InventoryManager inventoryManager;
    public QuestSystem questSystem;
    private bool questActivated = false;
    SaveData data = new SaveData();
    private CameraOffsetAnimator cameraOffsetAnimator;

    public void Start()
    {
        cameraOffsetAnimator = FindObjectOfType<CameraOffsetAnimator>();
        audioController = FindObjectOfType<AudioController>();
        questActivated = false;
        powerStone = FindObjectOfType<PowerStone>(); // Додайте цей рядок
        player = FindObjectOfType<Player>();
        dialogReader = FindObjectOfType<DialogReader>();
        pauseMenu.ToggleCursor(false);
        WizzardController wizard = FindObjectOfType<WizzardController>();
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
            player.health = data.playerHealth;
            player.healthSlider.value = player.health;
            PlayerPrefs.DeleteKey("LoadedPlayerPositionX");
            PlayerPrefs.DeleteKey("LoadedPlayerPositionY");
            PlayerPrefs.DeleteKey("LoadedPlayerPositionZ");

            PlayerPrefs.DeleteKey("LoadedCurrentDialogIndex");
            PlayerPrefs.DeleteKey("LoadedCurrentSentenceIndex");
            // загрузка чаклуна
            if (wizard != null)
            {
                float wizardX = PlayerPrefs.GetFloat("LoadedWizardPositionX");
                float wizardY = PlayerPrefs.GetFloat("LoadedWizardPositionY");
                float wizardZ = PlayerPrefs.GetFloat("LoadedWizardPositionZ");
                Vector3 loadedWizardPosition = new Vector3(wizardX, wizardY, wizardZ);
                wizard.transform.position = loadedWizardPosition;
                wizard.Wall.SetActive(data.wallActive);
                wizard.dialogComplete = data.dialogCompleted; // відновлюємо стан діалогу
                dialogReader.SetCurrentDialogIndex(loadedCurrentDialogIndex);
                dialogReader.SetCurrentSentenceIndex(loadedCurrentSentenceIndex);
                PlayerPrefs.DeleteKey("LoadedWizardPositionX");
                PlayerPrefs.DeleteKey("LoadedWizardPositionY");
                PlayerPrefs.DeleteKey("LoadedWizardPositionZ");
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

            // Завантаження радіусів монологу
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
            if (questSystem != null)
            {
                for (int i = 0; i < data.questStates.Count; i++)
                {
                    questSystem.questList[i].IsActive = data.questStates[i].IsActive;
                    questSystem.questList[i].IsComplete = data.questStates[i].IsComplete;
                    inventoryManager.questActivated = data.questActivated;
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

            // загрузка стану боса
            if (boss != null)
            {
                boss.LoadBossState();
            }

            // Завантаження стану інвентаря
            if (inventoryManager != null)
            {
                if (data.isPowerStoneInInventory)
                {
                    inventoryManager.AddPowerStoneToInventory();
                    inventoryManager.questActivated = data.questActivated;
                }
                else
                {
                    inventoryManager.powerStone.gameObject.SetActive(data.isPowerStoneActive);
                }
                inventoryManager.powerStone.transform.position = data.powerStonePosition;

            }

            // Завантаження стану камери
            if (cameraOffsetAnimator != null)
            {
                if (PlayerPrefs.HasKey("CameraPositionX") && PlayerPrefs.HasKey("CameraPositionY") && PlayerPrefs.HasKey("CameraPositionZ") && PlayerPrefs.HasKey("isAnimationPlayed"))
                {
                    cameraOffsetAnimator.isAnimationPlayed = data.isAnimationPlayed;
                    float CameraX = PlayerPrefs.GetFloat("CameraPositionX");
                    float CameraY = PlayerPrefs.GetFloat("CameraPositionY");
                    float CameraZ = PlayerPrefs.GetFloat("CameraPositionZ");
                    cameraOffsetAnimator.virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = new Vector3(CameraX, CameraY, CameraZ);
                    cameraOffsetAnimator.isCameraAnimating = data.isCameraAnimating;
                    // Якщо анімація вже була відтворена, відключаємо StartCanvasGroup
                    if (cameraOffsetAnimator.isAnimationPlayed)
                    {
                        StartCanvasGroup.gameObject.SetActive(false);
                    }
                }
            }

        }
    }

    public void SavePlayerProgress()
    {
        pauseMenu.PlaySaveAnimation();
        data.playerPosition = player.transform.position;
        data.playerHealth = player.health;
        data.currentScene = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("LoadedCurrentDialogIndex", dialogReader.GetCurrentDialogIndex());
        PlayerPrefs.SetInt("LoadedCurrentSentenceIndex", dialogReader.GetCurrentSentenceIndex());
        PlayerPrefs.Save();
        Debug.Log("Збереження гри...");

        // Збереження позиції чаклуна
        WizzardController wizard = FindObjectOfType<WizzardController>();
        if (wizard != null)
        {
            data.wizardPosition = wizard.transform.position;
            data.wallActive = wizard.Wall.activeSelf;
            data.dialogCompleted = wizard.dialogComplete;
        }

        // Збереження діалогу
        if (dialogReader != null)
        {
            data.currentDialogIndex = dialogReader.GetCurrentDialogIndex();
            data.currentSentenceIndex = dialogReader.GetCurrentSentenceIndex();
        }

        // Зберігання стану дерева
        TreeDestruction treeDestruction = FindObjectOfType<TreeDestruction>();
        if (treeDestruction != null)
        {
            data.treePosition = treeDestruction.transform.position;
            data.treeRotation = treeDestruction.transform.rotation;
            data.isTreeDestroyed = treeDestruction.IsDestroyed;
            data.emptyWallActive = treeDestruction.EmptyWallActive;
            data.InteractionRadius = treeDestruction.InteractionRadius;
        }

        // Збереження радіусів
        MonologueZone[] monologueZones = FindObjectsOfType<MonologueZone>();
        for (int i = 0; i < monologueZones.Length; i++)
        {
            PlayerPrefs.SetFloat("Zone" + monologueZones[i].zoneIndex + "Radius", monologueZones[i].radius);
        }

        // Збереження стану квестів
        data.questStates = new List<Quest>();
        foreach (var quest in questSystem.questList)
        {
            data.questStates.Add(new Quest { IsActive = quest.IsActive, IsComplete = quest.IsComplete });
            data.questActivated = inventoryManager.questActivated;
        }

        // Збереження стану підказок
        HintManager hintManager = FindObjectOfType<HintManager>();
        if (hintManager != null)
        {
            for (int i = 0; i < hintManager.hints.Count; i++)
            {
                PlayerPrefs.SetInt("Hint" + i, hintManager.hints[i].hasBeenShown ? 1 : 0);
            }
        }

        // Збереження стану боса
        if (boss != null)
        {
            boss.SaveBossState();
        }

        // Збереження стану інвентаря
        if (inventoryManager != null)
        {
            data.isPowerStoneInInventory = inventoryManager.IsPowerStoneInInventory();
            data.powerStonePosition = inventoryManager.powerStone.transform.position;
            data.isPowerStoneActive = inventoryManager.powerStone.gameObject.activeSelf;
            data.questActivated = inventoryManager.questActivated;
        }

        // Збереження стану камери
        if (cameraOffsetAnimator != null)
        {
            Vector3 cameraPosition = cameraOffsetAnimator.virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset;
            PlayerPrefs.SetFloat("CameraPositionX", cameraPosition.x);
            PlayerPrefs.SetFloat("CameraPositionY", cameraPosition.y);
            PlayerPrefs.SetFloat("CameraPositionZ", cameraPosition.z);
            data.isAnimationPlayed = cameraOffsetAnimator.isAnimationPlayed;
            data.isCameraAnimating = cameraOffsetAnimator.isCameraAnimating;
        }
        PlayerPrefs.Save();
        SaveManager.SaveGame(data);
    }

    private IEnumerator FadeInLevel()
    {
        yield return new WaitForSeconds(2);

        audioController = FindObjectOfType<AudioController>();
        if (audioController != null)
        {
            audioController.PlayAudio(levelMusic, null, null);
        }

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            StartCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            yield return null;
        }
        StartCanvasGroup.alpha = 0f;
    }

    private IEnumerator PlayAnimationAndDeactivateCanvas()
    {
        yield return StartCoroutine(FadeInLevel());
        StartCanvasGroup.gameObject.SetActive(false);
    }
}