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
        pauseMenu.ToggleCursor(false);
        WizardController wizard = FindObjectOfType<WizardController>();
        StartCoroutine(PlayAnimationAndDeactivateCanvas());

        if (SaveManager.IsGameSaved() && PlayerPrefs.HasKey("LoadedPlayerPositionX"))
        {
            float x = PlayerPrefs.GetFloat("LoadedPlayerPositionX");
            float y = PlayerPrefs.GetFloat("LoadedPlayerPositionY");
            float z = PlayerPrefs.GetFloat("LoadedPlayerPositionZ");
            Vector3 loadedPosition = new Vector3(x, y, z);
            playerMovement.transform.position = loadedPosition;
            PlayerPrefs.DeleteKey("LoadedPlayerPositionX");
            PlayerPrefs.DeleteKey("LoadedPlayerPositionY");
            PlayerPrefs.DeleteKey("LoadedPlayerPositionZ");

            int loadedCurrentDialogIndex = PlayerPrefs.GetInt("LoadedCurrentDialogIndex");
            int loadedCurrentSentenceIndex = PlayerPrefs.GetInt("LoadedCurrentSentenceIndex");
            dialogReader.SetCurrentDialogIndex(loadedCurrentDialogIndex);
            dialogReader.SetCurrentSentenceIndex(loadedCurrentSentenceIndex);
            PlayerPrefs.DeleteKey("LoadedCurrentDialogIndex");
            PlayerPrefs.DeleteKey("LoadedCurrentSentenceIndex");
            if (PlayerPrefs.HasKey("LoadedWizardPositionX"))
            {
                float wizardX = PlayerPrefs.GetFloat("LoadedWizardPositionX");
                float wizardY = PlayerPrefs.GetFloat("LoadedWizardPositionY");
                float wizardZ = PlayerPrefs.GetFloat("LoadedWizardPositionZ");
                Vector3 loadedWizardPosition = new Vector3(wizardX, wizardY, wizardZ);
                wizard.transform.position = loadedWizardPosition;

                PlayerPrefs.DeleteKey("LoadedWizardPositionX");
                PlayerPrefs.DeleteKey("LoadedWizardPositionY");
                PlayerPrefs.DeleteKey("LoadedWizardPositionZ");
            }
        }
    }

    public void SavePlayerProgress()
    {
        Debug.Log("Збереження гри...");
        pauseMenu.PlaySaveAnimation();
        SaveData data = new SaveData();
        data.playerPosition = playerMovement.transform.position;
        data.currentScene = SceneManager.GetActiveScene().buildIndex;

        // Збереження позиції чаклуна
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