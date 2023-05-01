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
        }
    }


    public void SavePlayerProgress()
    {
        Debug.Log("Збереження гри...");
        pauseMenu.PlaySaveAnimation();
        SaveData data = new SaveData();
        data.playerPosition = playerMovement.transform.position;
        data.currentScene = SceneManager.GetActiveScene().buildIndex;
        data.currentDialogIndex = dialogReader.GetCurrentDialogIndex();
        data.currentSentenceIndex = dialogReader.GetCurrentSentenceIndex();

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