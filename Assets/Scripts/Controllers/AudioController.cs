using System.Collections;
using UnityEngine;
public class AudioController : MonoBehaviour
{
    public AudioSource musicSource; // Посилання на AudioSource для відтворення музикі
    public AudioSource soundSource; // Посилання на AudioSource для відтворення звукових ефектів
    public AudioSource environmentSource; // Посилання на AudioSource для відтворення звукових ефектів оточення

    public void Start()
    {
        environmentSource.volume = 0.5f;
    }

    public void PlayAudio(AudioClip music, AudioClip sound, AudioClip environment)
    {
        // Відтворення саунду
        if (sound != null)
        {
            soundSource.clip = sound;
            soundSource.Play();
        }
        // Перемикач саундів
        // Якщо саунд не нульовий і відрізняється від поточного саунду, згасне поточна музика та перейде на нову музику
        if (music != null && musicSource.clip != music)
        {
            StartCoroutine(FadeOutAndSwitchMusic(music));

        }
        if (environment != null && environmentSource.clip != environment)
        {
            StartCoroutine(FadeOutAndSwitchEnvironment(environment));
        }
    }

    private IEnumerator FadeOutAndSwitchMusic(AudioClip music)
    {
        // Поступове згасання поточної музики
        if (musicSource.clip != null)
        {
            float startingVolume = musicSource.volume; // зберігати початкову гучність музики
            while (musicSource.volume > 0) // зменшення гучності музики до нуля
            {
                musicSource.volume -= startingVolume * Time.deltaTime / 1f;
                yield return null;
            }
        }
        else
        {
            musicSource.volume = 0;
        }

        // Перейти на нову музику і поступово збільшувати її гучність
        musicSource.clip = music; // Вставновлюємо новий музичний кліп
        musicSource.Play();
        while (musicSource.volume < 1) // збільшення гучності музики до 1
        {
            musicSource.volume += Time.deltaTime / 1f;
            yield return null;
        }
    }
    private IEnumerator FadeOutAndSwitchEnvironment(AudioClip music)
    {
        // Поступове згасання поточної музики
        if (environmentSource.clip != null)
        {
            float startingVolume = environmentSource.volume; // зберігати початкову гучність музики
            while (environmentSource.volume > 0) // зменшення гучності музики до нуля
            {
                environmentSource.volume -= startingVolume * Time.deltaTime / 0.5f;
                yield return null;
            }
        }
        else
        {
            environmentSource.volume = 0;
        }
        // Перейти на нову музику і поступово збільшувати її гучність
        environmentSource.clip = music; // Вставновлюємо новий музичний кліп
        environmentSource.Play();
        while (environmentSource.volume < 0.5f)
        {
            environmentSource.volume += Time.deltaTime / 0.5f;
            yield return null;
        }
    }

    public void StopAllAudio()
    {
        soundSource.Stop();
        StartCoroutine(FadeOutAndStop(environmentSource, 0.5f));
    }

    private IEnumerator FadeOutAndStop(AudioSource audioSource, float fadeDuration)
    {
        float startingVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startingVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startingVolume;
    }


}