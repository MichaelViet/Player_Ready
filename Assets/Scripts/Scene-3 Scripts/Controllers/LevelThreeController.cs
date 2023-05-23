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
    public InventoryUI inventoryUI;
    private bool questActivated = false;
    public bool fadeInCalled = false;
    private PlayerController playerController;
    private bool hintShown = false; // Відслідковує, чи був показаний підказка
    public SaveData data = new SaveData();
    private Item item;

    public static LevelThreeController instance;
    void Awake()
    {
        // Перевіряємо, чи існує екземпляр класу LevelThreeController
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            // Якщо екземпляр уже існує, знищуємо поточний
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        // Ініціалізуємо змінні та компоненти
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
        // Перевіряємо, чи гра не на паузі
        if (PauseMenu.isPaused)
        {
            return;
        }

        // Перевіряємо, чи закінчився поточний діалог і фейд-ін не був викликаний
        if (dialogReader.currentDialogIndex >= dialogReader.dialogData.dialog.Count && !fadeInCalled)
        {
            // Запускаємо фейд-ін та показуємо підказку з індексом 0
            questSystem.StartCoroutine(questSystem.FadeIn());
            fadeInCalled = true;
            hintManager.ShowHint(0);
            hintShown = true; // Оновлюємо, що підказка була показана
        }

        // Оновлюємо інтерфейс користувача після перевірки поточного індексу діалогу
        if (inventoryUI != null)
        {
            inventoryUI.UpdateUI();
        }
    }

    public void LoadPlayerProgress()
    {
        // Перевіряємо, чи є збережені дані та ключі координат гравця
        if (SaveManager.IsGameSaved() && PlayerPrefs.HasKey("PlayerPositionX") && PlayerPrefs.HasKey("PlayerPositionY") && PlayerPrefs.HasKey("PlayerPositionZ"))
        {
            data = SaveManager.LoadGame();

            // Завантаження стану діалогу
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
                dialogReader.bottomBarCanvasGroup.alpha = dialogComplete ? 0f : 1f;
                // Показуємо bottomBar, якщо діалог не завершено
            }

            // Завантаження предметів у інвентарі
            if (Inventory.instance != null && data.inventoryItems != null)
            {
                Inventory.instance.items = data.inventoryItems;
                for (int i = 0; i < Inventory.instance.items.Count; i++)
                {
                    Inventory.instance.items[i].isPickedUp = data.itemsPickedUpStates[i];
                }
                Inventory.instance.items = data.inventoryItems;
                if (Inventory.instance.onItemChangedCallback != null)
                    Inventory.instance.onItemChangedCallback.Invoke();
            }

            // Завантаження координат гравця
            if (playerMotor != null)
            {
                playerMotor.SetPosition(data.playerMotorPosition);
                playerController.dialogComplete = data.dialogCompleted;
                PlayerPrefs.DeleteKey("PlayerPositionX");
                PlayerPrefs.DeleteKey("PlayerPositionY");
                PlayerPrefs.DeleteKey("PlayerPositionZ");
                fadeInCalled = data.fadeInCalled;
            }

            // Завантаження стану квестів
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

            // Обробка стану предметів
            if (data.itemStates != null)
            {
                foreach (var itemState in data.itemStates)
                {
                    var item = Inventory.instance.items.Find(i => i.itemId == itemState.itemId);
                    if (item != null)
                    {
                        // Обробка стану предмету тут
                    }
                }
            }

            CharacterDialogue characterDialogue = FindObjectOfType<CharacterDialogue>();
            if (characterDialogue != null && characterDialogue.dialogJson != null)
            {
                characterDialogue.isPlayerInRange = data.isPlayerInRange;
                characterDialogue.hasDialogueFinished = data.hasDialogueFinished;
            }

        }
    }

    public void SavePlayerProgress()
    {
        Debug.Log("Збереження гри...");
        pauseMenu.PlaySaveAnimation();
        // Збереження індексу поточної сцени
        data.currentScene = SceneManager.GetActiveScene().buildIndex;

        // Збереження координат гравця
        data.playerMotorPosition = playerMotor.transform.position;
        data.dialogCompleted = playerController.dialogComplete;

        // Збереження поточного діалогу
        if (dialogReader != null)
        {
            data.currentDialogIndex = dialogReader.GetCurrentDialogIndex();
            data.currentSentenceIndex = dialogReader.GetCurrentSentenceIndex();
            PlayerPrefs.SetInt("LoadedCurrentDialogIndex", data.currentDialogIndex);
            PlayerPrefs.SetInt("LoadedCurrentSentenceIndex", data.currentSentenceIndex);
        }

        // Збереження предметів у інвентарі
        if (Inventory.instance != null)
        {
            data.inventoryItems = new List<Item>(Inventory.instance.items);
        }

        data.itemsPickedUpStates = new List<bool>();
        foreach (var item in Inventory.instance.items)
        {
            data.itemsPickedUpStates.Add(item.isPickedUp);
        }

        data.itemStates = new List<ItemState>();
        foreach (var item in Inventory.instance.items)
        {
            data.itemStates.Add(new ItemState(item.itemId, true));
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

        CharacterDialogue characterDialogue = FindObjectOfType<CharacterDialogue>();
        if (characterDialogue != null)
        {
            data.isPlayerInRange = characterDialogue.isPlayerInRange;
            data.hasDialogueFinished = characterDialogue.hasDialogueFinished;
        }

        SaveManager.SaveGame(data);
    }

}

