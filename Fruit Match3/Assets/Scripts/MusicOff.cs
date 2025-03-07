using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
