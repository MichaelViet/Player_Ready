using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStoryScene", menuName = "Data/New Story Scene")]
[System.Serializable]
public class StoryScene : GameScene
{
    public List<Sentence> sentences;
    public Sprite background;
    public GameScene nextScene;

    [System.Serializable]
    public struct Sentence
    {
        AudioSource musicSource; // Посилання на AudioSource для відтворення музикі
        AudioSource soundSource; // Посилання на AudioSource для відтворення звукових ефектів
        AudioSource environmentSource; // Посилання на AudioSource для відтворення звукових ефектів оточення
        public string text;
        public Speaker speaker;
        public List<Action> actions;

        public AudioClip music;
        public AudioClip sound;
        public AudioClip environment;

        [System.Serializable]
        public struct Action
        {
            public Speaker speaker;
            public int spriteIndex;
            public Type actionType;
            public Vector2 coords;
            public float moveSpeed;

            [System.Serializable]
            public enum Type
            {
                NONE,
                APPEAR,
                MOVE,
                DISAPPEAR
            }
        }
    }


}
// Визначає базовий клас для ігрової сцени
public class GameScene : ScriptableObject { }
