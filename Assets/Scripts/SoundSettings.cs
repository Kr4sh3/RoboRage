using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;


public class SoundSettings : MonoBehaviour
{
    [SerializeField] Slider _soundSlider;
    [SerializeField] AudioMixer _masterMixer;

    private void Start()
    {
        SetVolume(PlayerPrefs.GetFloat("SavedMasterVolume", 100));
    }

    public void SetVolume(float value)
    {
        if (value < 1)
        {
            value = .001f;
        }

        RefreshSlider(value);
        PlayerPrefs.SetFloat("SavedMasterVolume", value);
        _masterMixer.SetFloat("MasterVolume", Mathf.Log10(value / 100) * 20f);
    }

    public void SetVolumeFromSlider()
    {
        SetVolume(_soundSlider.value);
    }

    public void RefreshSlider(float _value)
    {
        _soundSlider.value = _value;
    }
}
