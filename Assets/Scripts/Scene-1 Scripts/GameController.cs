using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class GameController : MonoBehaviour
{
    public GameScene currentScene; // Посилання на поточну сцену, що відтворюється
    public BottomBarController bottomBar; // Посилання на компонент BottomBarController
    public SpriteSwitcher backgroundController; // Посилання на компонент SpriteSwitcher
    public ChooseController chooseController; // Посилання на компонент ChooseController
    public AudioController audioController; // Посилання на компонент AudioController

    private bool leftMouseButtonEnabled = true;
    private bool rightMouseButtonEnabled = true;
    private bool spacebarEnabled = true;
    private BasePauseMenu basePauseMenu;
    public static bool GameIsPaused = false;
    public DataHolder data;

    public Image bottomBarImage;

    public TextMeshProUGUI textObject1;
    public TextMeshProUGUI textObject2;
    public GameObject panel;
    public Animator transition;
    private State state = State.IDLE; // Enum для збереження поточного стану гри
    private List<StoryScene> history = new List<StoryScene>(); // Список для збереження історії відтворених сцен
    private enum State // Enum для зберігання можливих станів гри
    {
        IDLE, ANIMATE, CHOOSE
    }

    // Вивантажуємо всі невикористані ресурси з пам'яті
    private void UnloadUnusedAssets()
    {
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
    }
    void Start()
    {
        basePauseMenu = FindObjectOfType<BasePauseMenu>();
        basePauseMenu.ToggleCursor(true);
        StartLevel();
        UnloadUnusedAssets();
        if (SaveManager.IsGameSaved())
        {
            SaveData data = SaveManager.LoadGame();
            data.prevScenes.ForEach(scene =>
            {
                history.Add(this.data.scenes[scene] as StoryScene);
            });
            if (history.Count > 0) // Перевірте, чи історія не порожня
            {
                currentScene = history[history.Count - 1];
                history.RemoveAt(history.Count - 1);
                bottomBar.SetSentenceIndex(data.sentence - 1);
            }
        }

        // Перевірка якщо поточна сцена є StoryScene
        if (currentScene is StoryScene)
        {
            StoryScene storyScene = currentScene as StoryScene; // Транслюємо поточну сцену до StoryScene і зберігаємо її у змінній
            history.Add(storyScene); // Додати storyScene до списку history
            bottomBar.PlayScene(storyScene, bottomBar.GetSentenceIndex()); // Відтворення сюжетної сцени за допомогою BottomBarController
            backgroundController.SetImage(storyScene.background); // Встановлюємо фонове зображення за допомогою компонента SpriteSwitcher
            PlayAudio(storyScene.sentences[bottomBar.GetSentenceIndex()]); // Відтворення звукової доріжки для сцени
        }
    }

    IEnumerator StartLevel()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
    }

    void Update()
    {
        if (state != State.IDLE || PauseMenuController.isPaused)
        {
            return;
        }
        bool isClickingIgnoredUI = false;
        if (EventSystem.current.IsPointerOverGameObject())
        {
            GameObject clickedObject = EventSystem.current.currentSelectedGameObject;
            if (clickedObject != null && clickedObject.CompareTag("IgnoreTextOutput"))
            {
                isClickingIgnoredUI = true;
            }
        }

        // Если все объекты активны и пользователь нажал левую кнопку мыши
        if (textObject1.gameObject.activeSelf && textObject2.gameObject.activeSelf && bottomBarImage.enabled && panel.gameObject.activeSelf && Input.GetMouseButtonDown(0))
        {
            if (!isClickingIgnoredUI && (Input.GetKeyDown(KeyCode.Space) && spacebarEnabled && leftMouseButtonEnabled && rightMouseButtonEnabled || leftMouseButtonEnabled))
            {
                if (bottomBar.IsCompleted())
                {
                    bottomBar.StopTyping();
                    if (bottomBar.IsLastSentence())
                    {
                        audioController.StopAllAudio();
                        PlayScene((currentScene as StoryScene).nextScene);
                    }
                    else
                    {
                        bottomBar.PlayNextSentence();
                        PlayAudio((currentScene as StoryScene).sentences[bottomBar.GetSentenceIndex()]);
                    }
                }
                else
                {
                    bottomBar.SpeedUp();
                }
            }
        }
        else if (Input.GetMouseButtonDown(0)) // Если объекты неактивны и пользователь нажал левую кнопку мыши
        {
            ToggleObjects();
        }

        if (Input.GetMouseButtonDown(2))
        {
            ToggleObjects();
        }
    }

    public void ToggleObjects()
    {
        bool setActive = !textObject1.gameObject.activeSelf;

        bottomBarImage.enabled = setActive;
        textObject1.gameObject.SetActive(setActive);
        textObject2.gameObject.SetActive(setActive);
        leftMouseButtonEnabled = setActive;
        rightMouseButtonEnabled = setActive;
        spacebarEnabled = setActive;
        panel.gameObject.SetActive(setActive);
    }

    public void SaveGame()
    {
        Debug.Log("Збереження гри...");
        basePauseMenu.PlaySaveAnimation();
        List<int> historyIndicies = new List<int>();
        history.ForEach(scene =>
        {
            historyIndicies.Add(this.data.scenes.IndexOf(scene));
        });
        SaveData data = new SaveData
        {
            sentence = bottomBar.GetSentenceIndex(),
            prevScenes = historyIndicies
        };
        SaveManager.SaveGame(data);
    }

    // PlayScene метод дозволяє відтворити певну сцену в грі
    // Якщо для прапорця isAnimated встановлено значення true, сцена відтворюватиметься з анімацією
    public void PlayScene(GameScene scene, int sentenceIndex = -1, bool isAnimated = true)
    {
        StartCoroutine(SwitchScene(scene, sentenceIndex, isAnimated)); // Запуск coroutine (співпрограми) для перемикання на вказану сцену
    }

    // Ця корутина обробляє фактичне перемикання сцен
    private IEnumerator SwitchScene(GameScene scene, int sentenceIndex = -1, bool isAnimated = true)
    {
        state = State.ANIMATE; // Встановити стан на ANIMATE, який вказує на те, що анімація триває
        currentScene = scene; // Встановлюємо поточну сцену на вказану сцену


        if (isAnimated) // Якщо isAnimated = true, ховаємо bottomBar і чекаємо 1 секунду
        {
            bottomBar.Hide();
            yield return new WaitForSeconds(1f);
        }

        // Якщо сцена представляє собою StoryScene, відтворюємо вказане речення та оновлюємо фонове зображення та нижню панель
        if (scene is StoryScene)
        {
            StoryScene storyScene = scene as StoryScene; // Транслюємо сцену до StoryScene і додаємо її до списку історії
            history.Add(storyScene);
            PlayAudio(storyScene.sentences[sentenceIndex + 1]); // Відтворення аудіо 

            // Якщо isAnimated = true, змінюємо фонове зображення з анімацією. В іншому випадку просто встановлюємо зображення
            if (isAnimated)
            {
                backgroundController.SwitchImage(storyScene.background);
                yield return new WaitForSeconds(1f);
            }
            else
            {
                backgroundController.SetImage(storyScene.background);
            }

            // Очищаємо текст у bottomBar та показуємо його з анімацією чи без неї залежно від прапорця isAnimated
            bottomBar.ClearText();
            if (isAnimated)
            {
                bottomBar.Show();
                yield return new WaitForSeconds(1f);
            }
            bottomBar.PlayScene(storyScene, sentenceIndex, isAnimated);
            // Встановити стан IDLE, який вказує на те, що сцена готова прийняти введення від гравця
            state = State.IDLE;
        }
        // Якщо сцена представляє собою ChooseScene, встановлюємо стан на CHOOSE та показуємо вибір гравцеві
        else if (scene is ChooseScene)
        {
            state = State.CHOOSE;
            chooseController.SetupChoose(scene as ChooseScene);
        }
    }
    // Цей метод відтворює аудіо для певного речення в StoryScene
    private void PlayAudio(StoryScene.Sentence sentence)
    {
        audioController.PlayAudio(sentence.music, sentence.sound, sentence.environment);
    }
}
