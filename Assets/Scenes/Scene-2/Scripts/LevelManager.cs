using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public AudioClip levelMusic; // Музика для рівня
    public AudioClip levelAmbience; // Звук оточення для рівня
    public BasePauseMenu pauseMenu;
    private AudioController audioController;
    public CanvasGroup canvasGroup;
    public float fadeDuration = 3.0f;
    public float musicFadeDuration = 2.0f;
    public PlayerMovement playerMovement;
    public DialogReader dialogReader;

    public void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        dialogReader = FindObjectOfType<DialogReader>();
        WizardController wizard = FindObjectOfType<WizardController>();

        if (dialogReader == null)
        {
            Debug.LogError("Не знайдено компонента DialogReader.");
            return;
        }
        pauseMenu.ToggleCursor(false);
        StartCoroutine(PlayAnimationAndDeactivateCanvas());

        // Завантаження збереження, якщо воно існує
        if (SaveManager.IsGameSaved())
        {
            SaveData data = SaveManager.LoadGame();

            // Завантаження позиції гравця
            playerMovement.transform.position = data.playerPosition;

            // Завантаження позиції чарівника
            if (wizard != null)
            {
                wizard.transform.position = data.wizardPosition;
            }

            // Завантаження індексів діалогу та речення
            dialogReader.SetCurrentDialogIndex(data.currentDialogIndex);
            dialogReader.SetCurrentSentenceIndex(data.currentSentenceIndex);
        }
    }

    public void SavePlayerProgress()
    {
        Debug.Log("Збереження гри...");
        pauseMenu.PlaySaveAnimation();
        SaveData data = new SaveData();
        data.playerPosition = playerMovement.transform.position;
        data.currentScene = SceneManager.GetActiveScene().buildIndex;
        WizardController wizard = FindObjectOfType<WizardController>();
        if (wizard != null)
        {
            data.wizardPosition = wizard.transform.position;
        }
        else
        {
            Debug.LogError("Не знайдено компонента WizardController.");
        }
        if (dialogReader != null)
        {
            data.currentDialogIndex = dialogReader.GetCurrentDialogIndex();
            data.currentSentenceIndex = dialogReader.GetCurrentSentenceIndex();

        }
        else
        {
            Debug.LogError("Не знайдено компонента DialogReader.");
        }

        SaveManager.SaveGame(data);
    }

    private IEnumerator PlayAnimationAndDeactivateCanvas()
    {

        yield return StartCoroutine(FadeInLevel());

        canvasGroup.gameObject.SetActive(false);

        audioController = FindObjectOfType<AudioController>();

        if (audioController != null)
        {
            audioController.PlayAudio(levelMusic, null, levelAmbience);
        }
        else
        {
            Debug.LogWarning("Не знайдено компонента AudioController.");
        }
    }
    private IEnumerator FadeInLevel()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            {
                elapsedTime += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
                yield return null;
            }
            canvasGroup.alpha = 0f;
        }

    }
}