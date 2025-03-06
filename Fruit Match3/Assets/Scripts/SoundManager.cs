using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class SonudManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource _backgroundSource;
    [SerializeField] private AudioSource _soundEffectSource;

    [Header("Slider")]
    [SerializeField] private Slider _backgroundMusicSlider;
    [SerializeField] private Slider _soundEffectSlider;



}
