using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStoryScene", menuName = "Data/New Story Scene")]
[System.Serializable]
public class StoryScene : GameScene
{
    public List<Sentence> sentences; // Список речень у цій сцені
    public Sprite background; // Фонове зображення цієї сцени
    public GameScene nextScene; // Наступна сцена в грі

    // Визначаємо речення в storyScene
    [System.Serializable]
    public struct Sentence
    {
        public string text; // Текст речення
        public Speaker speaker; // Спікер 
        public List<Action> actions; // Список дій, пов'язаних з реченням

        public AudioClip music; // Музика та звук на фоні гри
        public AudioClip sound;

        // Визначає дію, пов’язану з реченням
        [System.Serializable]
        public struct Action
        {
            public Speaker speaker; // Спікер, пов'язаний з дією
            public int spriteIndex; // Спрайту, пов'язаний з дією
            public Type actionType; // Тип дії (наприклад, з'явитися, переміститися, зникнути)
            public Vector2 coords; // Координати дії
            public float moveSpeed; // Швидкість дії

            // Логіка поведінки спрайту на сцені
            [System.Serializable]
            public enum Type
            {
                NONE, // Ніяких дій
                APPEAR, // // З'являється спрайт
                MOVE, // Спрайт рухається
                DISAPPEAR // Спрайт зникає
            }
        }
    }
}
// Визначає базовий клас для ігрової сцени
public class GameScene : ScriptableObject { }
