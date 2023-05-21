using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static string SAVED_GAME = "/saveData.json";

    // Збереження гри
    public static void SaveGame(SaveData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + SAVED_GAME, json);
    }

    // Завантаження гри
    public static SaveData LoadGame()
    {
        string path = Application.persistentDataPath + SAVED_GAME;
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Debug.Log(Application.persistentDataPath);
            return JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return new SaveData();
        }
    }

    // Перевірка наявності збереженої гри
    public static bool IsGameSaved()
    {
        string path = Application.persistentDataPath + SAVED_GAME;
        return File.Exists(path);
    }

    // Видалення збереженої гри
    public static void ClearSavedGame()
    {
        string path = Application.persistentDataPath + SAVED_GAME;
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}

// Структура для зберігання даних гри
public struct SaveData
{
    public Vector3 cameraPosition; // Позиція камери
    public Vector3 playerPosition; // Позиція гравця
    public Vector3 playerMotorPosition; // Позиція гравця (сцена 3)
    public Vector3 wizzardPosition; // Позиція чарівника
    public Vector3 treePosition;// Позиція дерева
    public Vector3 powerStonePosition; // Позиція каменю сили
    public int sentence;// Номер речення
    public int currentScene;// Номер сцени
    public int currentDialogIndex;// Номер діалогу
    public int currentSentenceIndex; // Поточний індекс речення
    public int currentQuestIndex; // Поточний індекс квесту
    public int playerHealth; // Здоров'я гравця
    public float InteractionRadius; // Радіус взаємодії
    public float wizzardInteractionDistance; // Відстань взаємодії з чарівником
    public bool dialogCompleted; // Діалог завершено
    public bool isTreeDestroyed; // Дерево знищено
    public bool emptyWallActive; // Стан стіни
    public bool wallActive; // Стан стіни
    public bool isPowerStoneInInventory; // Камінь сили в інвентарі
    public bool isPowerStoneActive; // Камінь сили активний
    public bool questActivated; // Квест активовано
    public bool isAnimationPlayed; // Анімація відтворена
    public bool isCameraAnimating; // Анімація камери
    public bool isPortalAnimationActive; // Анімація порталу
    public List<Quest> questStates; // Список квестів
    public List<int> prevScenes; // Список попередніх сцен
    public Quaternion treeRotation;// Поворот дерева
    public bool fadeInCalled; // Анімація панелі квестів
}
