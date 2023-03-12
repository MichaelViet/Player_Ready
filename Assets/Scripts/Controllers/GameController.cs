using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public static bool GameIsPaused = false;
    public DataHolder data;

    public Image bottomBarImage;

    public TextMeshProUGUI textObject1;
    public TextMeshProUGUI textObject2;

    private State state = State.IDLE; // Enum для збереження поточного стану гри
    private List<StoryScene> history = new List<StoryScene>(); // Список для збереження історії відтворених сцен
    private enum State // Enum для зберігання можливих станів гри
    {
        IDLE, ANIMATE, CHOOSE
    }

    // Вивантажуємо всі невикористані ресурси з пам'яті
    public void UnloadUnusedAssets()
    {
        Resources.UnloadUnusedAssets();
    }

    void Start()
    {
        UnloadUnusedAssets();

        if (SaveManager.IsGameSaved())
        {
            SaveData data = SaveManager.LoadGame();
            data.prevScenes.ForEach(scene =>
            {
                history.Add(this.data.scenes[scene] as StoryScene);
            });
            currentScene = history[history.Count - 1];
            history.RemoveAt(history.Count - 1);
            bottomBar.SetSentenceIndex(data.sentence - 1);
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

    void Update()
    {
        Time.timeScale = 1;
        // Якщо стан гри не IDLE, повертаємося і не продовжуємо виконання методу Update
        if (state != State.IDLE)
        {
            return;
        }

        // Логіка тексту, якщо гравець натискає пробіл або ліву кнопку миші, текст міняється, сюжет рухається далі. 
        // Якщо гравець сховає bottomBar натиснувши колесико миші, то пробіл і ліва кнопка миші не будуть рухати сюжет.
        if (Input.GetKeyDown(KeyCode.Space) && bottomBar.gameObject.activeSelf && spacebarEnabled && leftMouseButtonEnabled && rightMouseButtonEnabled || Input.GetMouseButtonDown(0) && bottomBar.gameObject.activeSelf && leftMouseButtonEnabled)
        {
            // Якщо поточне речення завершено
            if (bottomBar.IsCompleted())
            {
                // Зупинняється введення та переходимо до наступного речення
                bottomBar.StopTyping();
                // Якщо це останнє речення в сцені відтворюємо наступну сцену
                if (bottomBar.IsLastSentence())
                {
                    audioController.StopAllAudio();
                    PlayScene((currentScene as StoryScene).nextScene);
                }
                else // Якщо ні відтворюємо наступне речення в поточній сцені
                {
                    bottomBar.PlayNextSentence();
                    PlayAudio((currentScene as StoryScene).sentences[bottomBar.GetSentenceIndex()]);
                }
            }
            else
            {
                bottomBar.SpeedUp(); // Прискорюємо введення поточного речення якщо гравець натискає ліву кнопку миші знову
            }
        }
        // Якщо гравець клацає правою кнопкою миші, вертаємося на попереднє речення
        else if (Input.GetMouseButtonDown(1) && bottomBar.gameObject.activeSelf && rightMouseButtonEnabled)
        {   // Перевірка, якщо це перше речення сцени
            if (bottomBar.IsFirstSentence())
            {
                // Якщо в історії більше однієї сцени
                if (history.Count > 1)
                {
                    bottomBar.StopTyping(); // Припинити вводити текст і сховати спрайти
                    bottomBar.HideSprites();

                    history.RemoveAt(history.Count - 1); // Видалити поточну сцену з історії
                    StoryScene scene = history[history.Count - 1]; // Отримати попередню сцену з історії
                    history.RemoveAt(history.Count - 1); // Видалення попередньої сцени з історії
                    PlayScene(scene, scene.sentences.Count - 2, false); // Відтворення попередньої сцени, починаючи з передостаннього речення
                }
            }
            // Повернутися до попереднього речення в поточній сцені
            else
            {
                bottomBar.GoBack();
            }
        }

        if (Input.GetMouseButtonDown(2))
        {
            ToggleObjects();
        }
    }

    private void ToggleObjects()
    {
        if (textObject1.gameObject.activeSelf)
        {
            bottomBarImage.enabled = false;
            textObject1.gameObject.SetActive(false);
            textObject2.gameObject.SetActive(false);
            leftMouseButtonEnabled = false;
            rightMouseButtonEnabled = false;
            spacebarEnabled = false;
        }
        else
        {
            textObject1.gameObject.SetActive(true);
            textObject2.gameObject.SetActive(true);
            bottomBarImage.enabled = true;
            leftMouseButtonEnabled = true;
            rightMouseButtonEnabled = true;
            spacebarEnabled = true;
        }
    }

    public void SaveGame()
    {
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

    // Ця співпрограма обробляє фактичне перемикання сцен
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
