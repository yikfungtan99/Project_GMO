using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private string masterVolumeParameter;
    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private Slider masterVolumeSlider;
    [Space(15)]
    [SerializeField] private string effectsVolumeParameter;
    [SerializeField] private Slider effectsVolumeSlider;
    [Space(15)]
    [SerializeField] private string musicVolumeParameter;
    [SerializeField] private Slider musicVolumeSlider;

    private void Awake()
    {
        masterVolumeSlider.onValueChanged.AddListener(HandleMasterVolumeChanged);
        effectsVolumeSlider.onValueChanged.AddListener(HandleEffectsVolumeChanged);
        musicVolumeSlider.onValueChanged.AddListener(HandleMusicVolumeChanged);
    }

    private void HandleMasterVolumeChanged(float vol)
    {
        masterMixer.SetFloat(masterVolumeParameter, vol);
    }
    
    private void HandleEffectsVolumeChanged(float vol)
    {
        masterMixer.SetFloat(effectsVolumeParameter, vol);
    }
    
    private void HandleMusicVolumeChanged(float vol)
    {
        masterMixer.SetFloat(musicVolumeParameter, vol);
    }
}
