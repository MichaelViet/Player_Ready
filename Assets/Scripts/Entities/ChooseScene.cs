// скріпт створює розвілку вибору, який використовується в першій 2D сцені для гравця

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewChooseScene", menuName = "Data/New Choose Scene")]
[System.Serializable]
public class ChooseScene : GameScene
{
    // Список вибору для ігрової сцени
    public List<ChooseLabel> labels;

    [System.Serializable]
    public struct ChooseLabel
    {
        // Вибір
        public string text;

        // Настпуна сцена яка буде відтворена після вибору 
        public StoryScene nextScene;
    }
}
