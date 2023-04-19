using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public AudioClip levelMusic; // Музика для рівня
    public AudioClip levelAmbience; // Звук оточення для рівня

    private AudioController audioController;

    private void Start()
    {
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
}
