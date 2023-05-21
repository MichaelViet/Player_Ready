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
    public Vector3 cameraPosition;
    public Vector3 playerPosition;
    public Vector3 playerMotorPosition;
    public Vector3 wizzardPosition;
    public Vector3 treePosition;
    public Vector3 powerStonePosition;
    public int sentence;
    public int currentScene;
    public int currentDialogIndex;
    public int currentSentenceIndex;
    public int currentQuestIndex;
    public int playerHealth;
    public float InteractionRadius;
    public float wizzardInteractionDistance;
    public bool dialogCompleted;
    public bool isTreeDestroyed;
    public bool emptyWallActive;
    public bool wallActive;
    public bool isPowerStoneInInventory;
    public bool isPowerStoneActive;
    public bool questActivated;
    public bool isAnimationPlayed;
    public bool isCameraAnimating;
    public bool isPortalAnimationActive;
    public List<Quest> questStates;
    public List<int> prevScenes;
    public Quaternion treeRotation;
}
