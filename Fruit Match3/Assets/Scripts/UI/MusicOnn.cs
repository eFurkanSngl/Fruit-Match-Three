using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicOnn : UIBTN
{
    [SerializeField] private AudioSource _backgroundMusic;

    protected override void OnClick()
    {
        OnMusicOn();
    }

    private void OnMusicOn()
    {
        _backgroundMusic.Play();
    }
}
