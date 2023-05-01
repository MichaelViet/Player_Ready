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
    public GameObject bottomBar;
    private Dialog dialogData;
    private int currentDialogIndex = 0;
    private int currentSentenceIndex = 0;

    void Start()
    {
        TextAsset dialogJson = Resources.Load<TextAsset>("dialog");
        string jsonString = dialogJson.text;
        dialogData = JsonUtility.FromJson<Dialog>(jsonString);
        DisplayDialog();
    }

    void Update()
    {
        if (BasePauseMenu.isPaused)
        {
            bottomBar.SetActive(false);
            return;
        }
        else
        {
            bottomBar.SetActive(true);
        }

        if (Input.GetMouseButtonDown(0))
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
                        bottomBar.SetActive(false); // вимкнення bottomBar
                        return; // вихід з методу
                    }
                }

                DisplayDialog();
            }
        }
    }

    public void DisplayDialog()
    {
        if (currentDialogIndex < dialogData.dialog.Count)
        {
            personNameText.text = dialogData.dialog[currentDialogIndex].speaker;
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
