using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleImage : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;
    [SerializeField] private Image _toggleImage;
    [SerializeField] private Sprite _musicOnSprite;
    [SerializeField] private Sprite _musicOffSprite;
    [SerializeField] private AudioSource _audioSource;

    private void Start()
    {
        _toggle.onValueChanged.AddListener(UpdateImage);
    }

    private void UpdateImage(bool isOn)
    {
        if (isOn)
        {
            _toggleImage.sprite = _musicOnSprite;
            _audioSource.Play();
        }
        else
        {
            _toggleImage.sprite= _musicOffSprite;
            _audioSource.Stop();
        }
    }
    private void OnDisable()
    {
        _toggle.onValueChanged.RemoveListener(UpdateImage);
    }
}