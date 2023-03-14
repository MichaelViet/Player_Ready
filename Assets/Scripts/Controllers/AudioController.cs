using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource musicSource; // Посилання на AudioSource для відтворення музикі
    public AudioSource soundSource; // Посилання на AudioSource для відтворення звукових ефектів
    public AudioSource environmentSource; // Посилання на AudioSource для відтворення звукових ефектів оточення

    StoryScene storyScene;
    public void PlayAudio(AudioClip music, AudioClip sound, AudioClip environment)
    {
        // Відтворення саунду
        if (sound != null)
        {
            soundSource.clip = sound;
            soundSource.Play();
        }
        if (environment != null)
        {
            environmentSource.clip = environment;
            environmentSource.Play();
        }
        else
        {
            environmentSource.clip = environment;
            environmentSource.Pause();
        }

        // Перемикач саундів
        // Якщо саунд не нульовий і відрізняється від поточного саунду, згасне поточна музика та перейде на нову музику
        if (music != null && musicSource.clip != music)
        {
            StartCoroutine(FadeOutAndSwitchMusic(music));
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
    public void StopAllAudio()
    {
        soundSource.Stop();
    }

}