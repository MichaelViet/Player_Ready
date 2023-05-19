using UnityEngine;
using System.Collections.Generic;

// Цей ScriptableObject представляє спікера в розмові з іменем і списком спрайтів
[CreateAssetMenu(fileName = "NewSpeaker", menuName = "Data/New Speaker")]
[System.Serializable]
public class Speaker : ScriptableObject
{
    public string speakerName; // Ім'я спікера
    public Color textColor; // Колір тексту кожкного спікера

    public List<Sprite> sprites; // Список спрайтів, які представляють спікера(спікерів)
    public SpriteController prefab;
}
