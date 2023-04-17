using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
public class BottomBarController : MonoBehaviour
{
    // Елементи інтерфейсу користувача для відображення діалогу та імені спікера
    public TextMeshProUGUI barText;
    public TextMeshProUGUI personNameText;

    // Слідкування за поточним реченням, яке відображається
    private int sentenceIndex = -1;
    private StoryScene currentScene;
    // Enum для відстеження стану текстової анімації
    private State state = State.COMPLETED;
    // Компонент аніматора для керування анімацією інтерфейсу
    private Animator animator;
    // Flag для відстеження видимості нижньої панелі
    private bool isHidden = false;

    // Словник для зберігання SpriteControllers для кожного спікера
    public Dictionary<Speaker, SpriteController> sprites;
    // Об'єкт для зберігання всіх екземплярів спрайтів
    public GameObject spritesPrefab;
    // Співпрограма для анімації ефекту друку
    private Coroutine typingCoroutine;
    // Швидкість виводу тексту
    private float speedFactor = 1f;

    bool leftMouseButtonEnabled = true;
    bool spacebarEnabled = true;

    // Enum для відстеження стану анімації
    private enum State
    {
        PLAYING, SPEEDED_UP, COMPLETED
    }

    private void Start()
    {
        sprites = new Dictionary<Speaker, SpriteController>(); // Ініціалізація словника спрайтів
        animator = GetComponent<Animator>(); // Отримати компонент Animator з об’єкта UI нижньої панелі
    }

    // Повертає індекс поточного речення
    public int GetSentenceIndex()
    {
        return sentenceIndex;
    }

    // Встановлює індекс поточного речення
    public void SetSentenceIndex(int sentenceIndex)
    {
        this.sentenceIndex = sentenceIndex;
    }

    // Приховує bottomBar
    public void Hide()
    {
        if (!isHidden)
        {
            animator.SetTrigger("Hide");
            isHidden = true;
        }
    }

    public void Show()
    {
        animator.SetTrigger("Show");
        isHidden = false;
    }

    public void ClearText()
    {
        barText.text = "";
        personNameText.text = "";
    }

    // Метод PlayScene використовується для відтворення певної сюжетної сцени
    public void PlayScene(StoryScene scene, int sentenceIndex = -1, bool isAnimated = true)
    {
        currentScene = scene;
        this.sentenceIndex = sentenceIndex; // використовується для визначення індексу речення, з якого починається відтворення
        PlayNextSentence(isAnimated);   // використовується, щоб визначити, чи повинен текст анімуватися під час введення
    }

    // Метод PlayNextSentence використовується для відтворення наступного речення в поточній сцені
    public void PlayNextSentence(bool isAnimated = true)
    {
        sentenceIndex++;
        PlaySentence(isAnimated); // isAnimated використовується, щоб визначити, чи повинен текст анімуватися під час введення
    }

    // Метод GoBack використовується для повернення до попереднього речення при натисканні правої кнопки миші
    public void GoBack()
    {
        sentenceIndex--;
        StopTyping();
        ClearText();
        HideSprites();
        PlaySentence(false);
    }
    // IsCompleted метод повертає, чи була сцена завершена
    public bool IsCompleted()
    {
        return state == State.COMPLETED || state == State.SPEEDED_UP;
    }

    // Цей метод повертає, чи є поточне речення останнім у сцені
    public bool IsLastSentence()
    {
        return sentenceIndex + 1 == currentScene.sentences.Count; // Перевіряємо, чи поточний індекс речення є останнім індексом у списку речень сцени
    }

    // Цей метод повертає, чи є поточне речення першим у сцені
    public bool IsFirstSentence()
    {
        return sentenceIndex == 0; // Перевіряємо, чи поточний індекс речення є першим індексом у списку речень сцени
    }

    // SpeedUp метод прискорює анімацію тексту при повторному натисканні лівої кнопки миші або пробілу
    public void SpeedUp()
    {
        state = State.SPEEDED_UP;
        speedFactor = 0.25f;
    }

    public void StopTyping()
    {
        state = State.COMPLETED;
        StopCoroutine(typingCoroutine);
    }

    public void HideSprites()
    {
        while (spritesPrefab.transform.childCount > 0)
        {
            DestroyImmediate(spritesPrefab.transform.GetChild(0).gameObject);
        }
        sprites.Clear();

    }

    // Цей метод використовується для відтворення речення
    private void PlaySentence(bool playAnimation = true)
    {
        speedFactor = 1f;
        var currentSentence = currentScene.sentences[sentenceIndex]; // Отримаємо поточне речення для відображення
        typingCoroutine = StartCoroutine(TypeText(currentSentence.text)); // Запуск співпрограми для введення тексту
        personNameText.text = currentSentence.speaker.speakerName; // Оновлення імені особи, яке буде відображено
        personNameText.color = currentSentence.speaker.textColor; // Оновити колір імені людини
        ActSpeakers(playAnimation); // Відтворення анімації для спікера, якщо вона вказана
    }

    // Ця функція відображає текст у textBar
    private IEnumerator TypeText(string text)
    {
        barText.text = ""; // Очистити текст у barText
        state = State.PLAYING; // Встановлюємо стан PLAYING

        // Перегляд кожного символу в тексті
        for (int i = 0; i < text.Length; i++)
        {
            barText.text += text[i]; // Додаємо символ до barText
            yield return new WaitForSeconds(speedFactor * 0.05f);

            // Перевірка, чи стан змінився на COMPLETED
            if (state == State.COMPLETED)
            {
                break; // Якщо так, виходимо з циклу
            }
        }
        // Встановити стан COMPLETED
        state = State.COMPLETED;
    }

    // Ця функція виконує дії для всіх спікерів у поточній сцені
    private void ActSpeakers(bool playAnimation = true)
    {

        var actions = currentScene.sentences[sentenceIndex].actions; // Отримаємо список дій для поточного речення
        foreach (var action in actions) // Перегляд кожної дії в списку
        {
            ActSpeaker(action, playAnimation); // Викликаємо функцію ActSpeaker для кожної дії
        }
    }

    // Ця функція виконує дію для спікера
    private void ActSpeaker(StoryScene.Sentence.Action action, bool playAnimation = true)
    {
        // Отримаємо SpriteController для спікера
        SpriteController controller;
        if (!sprites.TryGetValue(action.speaker, out controller))
        {
            // Якщо контролер не існує, створіть його з префабу спікера
            GameObject prefab = action.speaker.prefab.gameObject;
            GameObject instance = Instantiate(prefab, spritesPrefab.transform);
            controller = instance.GetComponent<SpriteController>();
            // Додаємо контролер до словника
            sprites.Add(action.speaker, controller);
        }

        switch (action.actionType)
        {
            case StoryScene.Sentence.Action.Type.APPEAR:
                controller.Setup(action.speaker.sprites[action.spriteIndex]); // Налаштування контролера з відповідним спрайтом
                controller.Show(action.coordinates, playAnimation); // Показувати контролер з анімацією або без неї
                break;
            case StoryScene.Sentence.Action.Type.MOVE:
                controller.Move(action.coordinates, action.moveSpeed, playAnimation);
                break;
            case StoryScene.Sentence.Action.Type.DISAPPEAR:
                controller.Hide(playAnimation);
                break;
        }
        controller.SwitchSprite(action.speaker.sprites[action.spriteIndex], playAnimation);
    }


}
