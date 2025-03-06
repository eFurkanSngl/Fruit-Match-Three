using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicOff : UIBTN
{
    [SerializeField] private AudioSource _backgroundMusic;

    protected override void OnClick()
    {
        OnMusicOff();
    }

    private void OnMusicOff()
    {
        _backgroundMusic.Stop();
    }
}
