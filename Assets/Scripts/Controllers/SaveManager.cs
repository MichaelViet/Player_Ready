using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static string SAVED_GAME = "/saveData.json";

    public static void SaveGame(SaveData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + SAVED_GAME, json);
    }

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

    public static bool IsGameSaved()
    {
        string path = Application.persistentDataPath + SAVED_GAME;
        return File.Exists(path);
    }

    public static void ClearSavedGame()
    {
        string path = Application.persistentDataPath + SAVED_GAME;
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}

public struct SaveData
{
    public List<int> prevScenes;
    public int sentence;
    public Vector3 playerPosition;
    public int currentScene;
    public int currentDialogIndex;
    public int currentSentenceIndex;
    public bool dialogCompleted;
    public Vector3 wizardPosition;
    public Vector3 treePosition;
    public Quaternion treeRotation;
    public bool isTreeDestroyed;
    public bool emptyWallActive;
    public float InteractionRadius;
    public bool wallActive;
    public float wizardInteractionDistance;
    public List<bool> monologueZonesCompleted;
    public List<float> monologueZonesRadii;
    public int currentQuestIndex;
    public List<Quest> questStates;
    public List<bool> hintStates;
    public bool isPowerStoneInInventory;
    public Vector3 powerStonePosition;
    public bool isPowerStoneActive;
    public bool questActivated;
    public bool isAnimationPlayed;
    public bool isCameraAnimating;
    public int playerHealth;
}
