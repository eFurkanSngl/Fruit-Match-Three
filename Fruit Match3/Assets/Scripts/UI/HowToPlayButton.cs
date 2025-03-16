using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlayButton :UIBTN
{
    [SerializeField] private GameObject _howToPlayScene;
    protected override void OnClick()
    {
        OnLoadScene();
    }

    private void OnLoadScene()
    {
        _howToPlayScene.SetActive(true);
    }
}
