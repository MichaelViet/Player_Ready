using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static string SAVED_GAME = "savedGame";

    public static void SaveGame(SaveData data)
    {
        PlayerPrefs.SetString(SAVED_GAME, JsonUtility.ToJson(data));
    }

    public static SaveData LoadGame()
    {
        return JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString(SAVED_GAME));
    }

    public static bool IsGameSaved()
    {
        return PlayerPrefs.HasKey(SAVED_GAME);
    }

    public static void ClearSavedGame()
    {
        PlayerPrefs.DeleteKey(SAVED_GAME);
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
}