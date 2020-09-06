using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    public AudioMixer m_audioMixer;
    public Slider m_musicSlider;
    public Slider m_soundFXSlider;

    private void Awake()
    {
        m_soundFXSlider.value = PlayerPrefs.GetFloat("SFX", 0.50f);
        m_musicSlider.value = PlayerPrefs.GetFloat("Music", 0.50f);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }

    public void SetSFXVolume(float volume)
    {
        m_audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFX", m_soundFXSlider.value);
    }

    public void SetMusicVolume(float volume)
    {
        m_audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("Music", m_musicSlider.value);
    }
}
