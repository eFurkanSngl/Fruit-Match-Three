using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventListenerMono : MonoBehaviour
{
    private void Start()
    {
        RegisterEvents();
    }

    private void OnEnable()
    {
        RegisterEvents();
    }

    private void OnDisable()
    {
        UnRegisterEvents();
    }

    protected abstract void UnRegisterEvents();
    protected abstract void RegisterEvents();
    
}
