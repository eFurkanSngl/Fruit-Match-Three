using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

public class PauseBtn : UIBTN
{
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private RectTransform _settingsTransform;
    [SerializeField] private RectTransform _settingsBtnTransform;
    [SerializeField] private float _topPosY, _middlePosY;
    [SerializeField] private float _tweenDuration;
    [SerializeField] private CanvasGroup _canvasGroup;

    private void ShowSettingsMenu()
    {
        _settingsMenu.SetActive(true);
        SettingsPanelIntro();
        //Time.timeScale = 0f;
        //Debug.Log("Time is stopped");
        GameUIEvents.OnPause?.Invoke();

    }
    protected override void OnClick()
    {
        ShowSettingsMenu();
    }

    private void SettingsPanelIntro()
    {
        _settingsBtnTransform.DOAnchorPosX(1250, _tweenDuration).SetUpdate(true);
        _canvasGroup.DOFade(1, _tweenDuration).SetUpdate(true);
        _settingsTransform.DOAnchorPosY(_middlePosY, _tweenDuration).SetUpdate(true);
    }

    private void SettingsPanelOutro()
    {
        _canvasGroup.DOFade(0,_tweenDuration).SetUpdate(true);
        _settingsBtnTransform.DOAnchorPosX(720,_tweenDuration).SetUpdate(true);
       _settingsTransform.DOAnchorPosY(_topPosY, _tweenDuration).SetUpdate(true);
    }

    private void OnEnable()
    {
        UIBtnEvents.SettingsUIEvents += SettingsPanelOutro;
    }
    private void OnDisable()
    {
        UIBtnEvents.SettingsUIEvents -= SettingsPanelOutro;
    }

 
}
