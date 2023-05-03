using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    private Dictionary<string, Color> speakerColors = new Dictionary<string, Color>
    {
        { "Джейкоб", new Color(0.898f, 0.3961f, 0.1725f, 1) }, // #E5652C:
        { "Чаклун", new Color(0.3098f, 0.2667f, 0.5451f, 1) } // #4F448B:
    };
    private Dialog dialogData;
    private int currentDialogIndex = 0;
    private int currentSentenceIndex = 0;
    public delegate void DialogCompleteAction();
    public event DialogCompleteAction OnDialogComplete;
    private void Awake()
    {
        TextAsset dialogJson = Resources.Load<TextAsset>("dialog");
        if (dialogJson != null)
        {
            string jsonString = dialogJson.text;
            dialogData = JsonUtility.FromJson<Dialog>(jsonString);
            Debug.Log("JSON loaded successfully: " + jsonString);
            bottomBarCanvasGroup.alpha = 1f; // Встановіть alpha на 1
            DisplayDialog();
        }
        else
        {
            Debug.LogError("Failed to load JSON file.");
        }

        if (PlayerPrefs.HasKey("LoadedCurrentDialogIndex") && PlayerPrefs.HasKey("LoadedCurrentSentenceIndex"))
        {
            SetCurrentDialogIndex(PlayerPrefs.GetInt("LoadedCurrentDialogIndex"));
            SetCurrentSentenceIndex(PlayerPrefs.GetInt("LoadedCurrentSentenceIndex"));
            DisplayDialog();
        }
    }
    void Update()
    {
        if (BasePauseMenu.isPaused)
        {
            bottomBarCanvasGroup.alpha = 0f;
            return;
        }

        if (Input.GetMouseButtonDown(0) && bottomBarCanvasGroup.alpha == 1) // Додайте перевірку на alpha
        {
            if (currentDialogIndex < dialogData.dialog.Count)
            {
                currentSentenceIndex++;

                if (currentSentenceIndex >= dialogData.dialog[currentDialogIndex].sentences.Count)
                {
                    currentDialogIndex++;
                    currentSentenceIndex = 0;
                    if (currentDialogIndex >= dialogData.dialog.Count) // перевірка на кінець діалогу
                    {
                        bottomBarCanvasGroup.alpha = 0f; // вимкнення bottomBar
                        OnDialogComplete?.Invoke();
                        return; // вихід з методу
                    }
                }

                DisplayDialog();
            }
        }
    }

    public void DisplayDialog()
    {
        if (currentDialogIndex < dialogData.dialog.Count && bottomBarCanvasGroup.alpha == 1) // Додайте перевірку на alpha
        {
            string speaker = dialogData.dialog[currentDialogIndex].speaker;
            personNameText.text = speaker;

            // Задайте колір тексту спікера, використовуючи словник
            if (speakerColors.ContainsKey(speaker))
            {
                personNameText.color = speakerColors[speaker];
            }
            Debug.Log($"Dialog index: {currentDialogIndex}, Sentence index: {currentSentenceIndex}");
            Debug.Log($"Sentence: {dialogData.dialog[currentDialogIndex].sentences[currentSentenceIndex]}");

            bottomBarText.text = dialogData.dialog[currentDialogIndex].sentences[currentSentenceIndex];
        }
    }
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