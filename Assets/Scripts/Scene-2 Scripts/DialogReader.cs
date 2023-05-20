using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

[System.Serializable]
public class DialogEntry
{
    public string speaker;
    public List<string> sentences;
}

[System.Serializable]
public class Dialog
{
    public List<DialogEntry> dialog;
}

public class DialogReader : MonoBehaviour
{
    public TextMeshProUGUI bottomBarText;
    public TextMeshProUGUI personNameText;
    public CanvasGroup bottomBarCanvasGroup;
    public Dialog dialogData;
    public TextAsset dialogJson;
    public List<SpeakerColor> speakerColors;

    private int currentDialogIndex = 0;
    private int currentSentenceIndex = 0;
    public delegate void DialogCompleteAction();
    public event DialogCompleteAction OnDialogComplete;
    private bool newDialogLoaded = false;
    public TextAsset dialogJsonAfterBossDies; // Додаємо новий JSON файл для діалогу після смерті босса

    [System.Serializable]
    public class SpeakerColor
    {
        public string speakerName;
        public Color color;
    }

    private void Start()
    {
        if (dialogJson != null)
        {
            string jsonString = dialogJson.text;
            dialogData = JsonUtility.FromJson<Dialog>(jsonString);
            Debug.Log("JSON loaded successfully: " + jsonString);
            bottomBarCanvasGroup.alpha = 1f;
        }
        else
        {
            Debug.LogError("Failed to load JSON file.");
        }

        if (PlayerPrefs.HasKey("LoadedCurrentDialogIndex") && PlayerPrefs.HasKey("LoadedCurrentSentenceIndex"))
        {
            SetCurrentDialogIndex(PlayerPrefs.GetInt("LoadedCurrentDialogIndex"));
            SetCurrentSentenceIndex(PlayerPrefs.GetInt("LoadedCurrentSentenceIndex"));
        }
        DisplayDialog();
    }
    private bool IsPointerOverUIElement()
    {
        // Перевіряємо, чи натискання миші відбувається на елемент UI
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }

        return false;
    }
    void Update()
    {
        if (BasePauseMenu.isPaused)
        {
            bottomBarCanvasGroup.alpha = 0f;
            return;
        }

        if (Input.GetMouseButtonDown(0) && bottomBarCanvasGroup.alpha == 1 && !IsPointerOverUIElement())
        {
            if (currentDialogIndex < dialogData.dialog.Count)
            {
                currentSentenceIndex++;

                if (currentSentenceIndex >= dialogData.dialog[currentDialogIndex].sentences.Count)
                {
                    currentDialogIndex++;
                    currentSentenceIndex = 0;
                    if (currentDialogIndex >= dialogData.dialog.Count)
                    {
                        bottomBarCanvasGroup.alpha = 0f;
                        OnDialogComplete?.Invoke();
                        return;
                    }
                }
                DisplayDialog();
            }
        }
    }
    public void LoadDialog(TextAsset dialogJsonFile)
    {
        string jsonString = dialogJsonFile.text;
        dialogData = JsonUtility.FromJson<Dialog>(jsonString);
        Debug.Log("JSON loaded successfully: " + jsonString);
        bottomBarCanvasGroup.alpha = 1f;
        SetCurrentDialogIndex(0);
        SetCurrentSentenceIndex(0);
        DisplayDialog();
        // Встановити новий прапорець завантаженого діалогового вікна на true
        newDialogLoaded = true;
        // Виклик DisplayDialog, щоб показати перше речення нового діалогу
        DisplayDialog();
    }
    public void DisplayDialog()
    {
        if (currentDialogIndex < dialogData.dialog.Count && bottomBarCanvasGroup.alpha == 1)
        {
            string speaker = dialogData.dialog[currentDialogIndex].speaker;
            personNameText.text = speaker;

            Color speakerColor = Color.white;
            foreach (var entry in speakerColors)
            {
                if (entry.speakerName == speaker)
                {
                    speakerColor = entry.color;
                    break;
                }
            }
            personNameText.color = speakerColor; Debug.Log($"Dialog index: {currentDialogIndex}, Sentence index: {currentSentenceIndex}");
            Debug.Log($"Sentence: {dialogData.dialog[currentDialogIndex].sentences[currentSentenceIndex]}");

            bottomBarText.text = dialogData.dialog[currentDialogIndex].sentences[currentSentenceIndex];
        }
    }

    // Додаємо відкритий метод для отримання поточного індексу діалогу
    public int GetCurrentDialogIndex()
    {
        return currentDialogIndex;
    }

    public int GetCurrentSentenceIndex()
    {
        return currentSentenceIndex;
    }

    public void SetCurrentDialogIndex(int index)
    {
        currentDialogIndex = index;
    }

    public void SetCurrentSentenceIndex(int index)
    {
        currentSentenceIndex = index;
    }
}