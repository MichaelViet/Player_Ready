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
        AudioSource musicSource;
        AudioSource soundSource;
        AudioSource environmentSource;
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
            public Vector2 coordinates;
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
