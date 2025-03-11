using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer _myMixer;
    [SerializeField] private Slider  _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    

    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetSfxVolume();
        }

    }
    public void SetSfxVolume()
    {
        float  sfxVolume = _sfxSlider.value;
        _myMixer.SetFloat("Sfx",Mathf.Log10(sfxVolume)*20);
        PlayerPrefs.SetFloat("SfxVolume",sfxVolume);
    }


    public void SetMusicVolume()
    {
        float volume = _musicSlider.value;
        _myMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume",volume);
    }

    private void LoadVolume()
    {
        _musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        _sfxSlider.value = PlayerPrefs.GetFloat("SfxVolume");
        SetMusicVolume();
        SetSfxVolume();
    }
}
