using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitBTN : UIBTN
{
    [SerializeField] private GameObject _closePanel;
  
    protected override void OnClick()
    {
        ClosePanel();
    }
    private  void ClosePanel()
    {
        _closePanel.SetActive(false);
        UIBtnEvents.SettingsUIEvents?.Invoke();
        Time.timeScale = 1.0f;
        Debug.Log("Time is started");
    }
}
