using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : UIBTN
{
    [SerializeField] private GameObject _settinsgPanel;
    protected override void OnClick()
    {
        OnActiveScene();
    }

    private void OnActiveScene()
    {
        _settinsgPanel.SetActive(true);

    }
}
