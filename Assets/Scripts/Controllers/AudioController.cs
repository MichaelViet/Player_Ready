using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource musicSource; // Посилання на AudioSource для відтворення музикі
    public AudioSource soundSource; // Посилання на AudioSource для відтворення звукових ефектів
    public AudioSource environmentSource; // Посилання на AudioSource для відтворення звукових ефектів оточення

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
    {   // Згасання поточної музики
        if (musicSource.clip != null)
        {
            while (musicSource.volume > 0)
            {
                musicSource.volume -= 0.05f;
                yield return new WaitForSeconds(0.05f);
            }
        }
        else
        {
            musicSource.volume = 0;
        }

        // Переключаємо на нову музику та відображаємо її
        musicSource.clip = music;
        musicSource.Play();

        // Згасання(Исчезновение) нової музики
        while (musicSource.volume < 0.5)
        {
            musicSource.volume += 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
    }
}