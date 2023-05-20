using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;
public class OptionsController : MonoBehaviour
{
    public TextMeshProUGUI musicValue;
    public TextMeshProUGUI soundValue;
    public TextMeshProUGUI environmentValue;
    public AudioMixer musicMixer;
    public AudioMixer soundMixer;
    public AudioMixer environmentMixer;

    public Slider musicSlider;
    public Slider soundSlider;
    public Slider environmentSlider;

    private int _window = 0;

    public void LateUpdate()
    {
        if (_window == 1)
        {
            _window = 0;
        }
        if (musicSlider.value != PlayerPrefs.GetFloat("musicVolume", 50f))
        {
            OnMucicChanged(musicSlider.value);
        }
        if (soundSlider.value != PlayerPrefs.GetFloat("soundVolume", 50f))
        {
            OnSoundChanged(soundSlider.value);
        }
        if (environmentSlider.value != PlayerPrefs.GetFloat("environmentVolume", 50f))
        {
            OnEnvironmentChanged(environmentSlider.value);
        }
    }

    private void Start()
    {
        // Зчитуємо збережені значення звуку та встановлюємо їх на слайдерах
        float musicVolume = PlayerPrefs.GetFloat("musicVolume", 100f);
        float soundVolume = PlayerPrefs.GetFloat("soundVolume", 100f);
        float environmentVolume = PlayerPrefs.GetFloat("environmentVolume", 100f);
        musicValue.SetText(musicVolume.ToString());
        soundValue.SetText(soundVolume.ToString());
        environmentValue.SetText(environmentVolume.ToString());
        musicSlider.value = musicVolume;
        soundSlider.value = soundVolume;
        environmentSlider.value = environmentVolume;
        OnMucicChanged(musicVolume);
        OnSoundChanged(soundVolume);
        OnEnvironmentChanged(environmentVolume);
    }

    public void OnMucicChanged(float value)
    {
        musicValue.SetText(value + "");
        float volume = value <= 0 ? -80 : Mathf.Lerp(0, -35, 1 - value / 100);
        musicMixer.SetFloat("volume", volume);
        PlayerPrefs.SetFloat("musicVolume", value);
    }

    public void OnSoundChanged(float value)
    {
        soundValue.SetText(value + "");
        float volume = value <= 0 ? -80 : Mathf.Lerp(0, -35, 1 - value / 100);
        soundMixer.SetFloat("volume", volume);
        PlayerPrefs.SetFloat("soundVolume", value);
    }
    public void OnEnvironmentChanged(float value)
    {
        environmentValue.SetText(value + "");
        float volume = value <= 0 ? -80 : Mathf.Lerp(0, -35, 1 - value / 100);
        environmentMixer.SetFloat("volume", volume);
        PlayerPrefs.SetFloat("environmentVolume", value);
    }
}