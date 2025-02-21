using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIBTN : EventListenerMono
{
    [SerializeField] private Button _button;

    protected override void RegisterEvents()
    {
        _button.onClick.AddListener(OnClick);
    }
    protected override void UnRegisterEvents()
    {
        _button.onClick.RemoveListener(OnClick);
    }
    protected abstract void OnClick();
}