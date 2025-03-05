using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Slider _timeSlider;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private float _sliderTime = 30f;
    [SerializeField] private GameObject _gameOverPanel;
    private bool _stopTimer= false;
    private WaitForSeconds _timer = new WaitForSeconds(1f);

    private bool _isPaused = false;  // oyun durmadý
    private bool _isWarning = false;

    private void Start()
    {
        _timeSlider.maxValue = _sliderTime;  // maks sliderTime kadar ol
        _timeSlider.value = _sliderTime; // Deðer de yine SliderTime dan alýyor
        StartTimeText();
        StartCoroutine(DelayStart());
    }

    private void StartTimeText()
    {
        if(_timeText != null)
        {
            _timeText.text = _sliderTime.ToString("0");
        }
        else
        {
            Debug.Log("EmptyText");
        }
    }

    IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(StartTime());
    }
    IEnumerator StartTime()
    {
        while (!_stopTimer)
        {
            if (_isPaused)  // burada false deðer döner devam eder oyun
            {
                yield return null;
                continue;
            }

            _sliderTime--;
            _timeSlider.value = _sliderTime;
            _timeText.text = Mathf.CeilToInt(_sliderTime).ToString();


            if(_sliderTime <= 10 && !_isWarning)
            {
                _isWarning = true;
                TimeWarningEffect();
            }

            if (_sliderTime <= 0)
            {
                _stopTimer = true;
                _sliderTime = 0;
                GameOver();
            }

            yield return _timer;
        }
    }
    private void TimeWarningEffect()
    {
        _timeText.color = Color.red;
        _timeText.transform.DOShakePosition(1f, 10f, 10, 90, false, true).SetLoops(-1, LoopType.Restart);
    }
    private void GameOver()
    {
        _gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        _gameOverPanel.SetActive(false);
        StopAllCoroutines();
        _timeText.text = _sliderTime.ToString();
        _sliderTime = 30f;
        _timeSlider.value = _sliderTime;
        _stopTimer = false;
        _isWarning = false;
        _timeText.color = Color.white;
        _timeText.transform.DOKill();
        StartCoroutine(StartTime());
    }

    private void OnPause()
    {
        _isPaused = true;  // oyun duraklatýldý.
        Time.timeScale = 0f;
    }
    private void OnResume()
    {
        _isPaused = false;
        Time.timeScale = 1f;
    }

    private void OnEnable()
    {
        RegisterEvents();
    }

   
    private void OnDisable()
    {
        UnRegisterEvents();
    }
    private void RegisterEvents()
    {
        GameUIEvents.OnPause += OnPause;
        GameUIEvents.OnResume += OnResume;
        GameUIEvents.GameUI += RestartGame;
    }

    private void UnRegisterEvents()
    {
        GameUIEvents.GameUI -= RestartGame;
        GameUIEvents.OnPause -= OnPause;
        GameUIEvents.OnResume -= OnResume;
    }
}
