using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Timer Settings")]
    [SerializeField] private Slider _timeSlider;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private float _sliderTime = 30f;

    [Header("Panels Settings")]
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _nextLevelPanel;
    [SerializeField] private TextMeshProUGUI _gameOverText;
    [SerializeField] private TextMeshProUGUI _nextLevelText;
    [SerializeField] private TextMeshProUGUI _nextlevelHighScore;
    [SerializeField] private TextMeshProUGUI _gameOverHighScore;


    [SerializeField] private AudioSource _gameOverAudio;
    [SerializeField] private AudioSource _nextLevelAudio;

    private bool _stopTimer= false;
    private WaitForSeconds _timer = new WaitForSeconds(1f);
    private Coroutine _coroutine;
    private ScoreManager _scoreManager;

    private bool _isPaused = false;  // oyun durmadý
    private bool _isWarning = false;

    private void Start()
    {
        _timeSlider.maxValue = _sliderTime;  // maks sliderTime kadar ol
        _timeSlider.value = _sliderTime; // Deðer de yine SliderTime dan alýyor
        StartTimeText();
        StartCoroutine(DelayStart());
        _scoreManager = FindObjectOfType<ScoreManager>();

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
        yield return _timer;
       _coroutine = StartCoroutine(StartTime());
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

            _sliderTime -= Time.deltaTime * 1.2f;
            _timeSlider.value = _sliderTime;
            _timeText.text = Mathf.CeilToInt(_sliderTime).ToString();


            if(_sliderTime <= 10 && !_isWarning)
            {
                _isWarning = true;
                TimeWarningEffect();
            }
            else if ( _sliderTime >= 10 && _isWarning)
            {
                _isWarning = false;
                _timeText.color = Color.white;
            }

            if (_sliderTime <= 0)
            {
                _stopTimer = true;
                _sliderTime = 0;
                GameOver();
            }

            yield return null;
        }
    }

    private void AddTime(float amountTime)
    {
        _sliderTime += amountTime;
        if(_sliderTime > 30f)
        {
            _sliderTime = 30f;
        }
    }
    private void TimeWarningEffect()
    {
        _timeText.color = Color.red;
        _timeText.transform.DOShakePosition(1f, 10f, 10, 90, false, true).
            SetLoops(-1, LoopType.Restart);
    }
    private void GameOver()
    {
        _scoreManager.SaveHighScore();
        if(_scoreManager.Score >= _scoreManager.TargetScore)
        {
            Debug.Log("Next Level Panel");
            _nextLevelPanel.SetActive(true);
            _nextLevelText.text = "Your Score: " + _scoreManager.Score;
            _nextlevelHighScore.text = "High Score: " + _scoreManager.HighScore;
            _nextLevelAudio.Play();
        }
        else
        {
            _gameOverPanel.SetActive(true);
            //Time.timeScale = 0f;
            StopAllCoroutines();
            _gameOverText.text = "Your Score: " + _scoreManager.Score;
            _gameOverHighScore.text = "High Score: " + _scoreManager.HighScore;

            _gameOverAudio.Play();
        }
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        _gameOverPanel.SetActive(false);
        _nextLevelPanel.SetActive(false) ;
        StopAllCoroutines(); // eski olaný sýfýrlamak için durduruyoruz
        _timeText.text = _sliderTime.ToString();
        _sliderTime = 30f;
        _timeSlider.value = _sliderTime;
        _stopTimer = false;
        _isWarning = false;
        _timeText.color = Color.white;
        _timeText.transform.DOKill();
        StartCoroutine(DelayStart());
        _scoreManager.ScoreText.text ="Score: " + 0.ToString();
        _scoreManager.ResetScore();
    }

    private void OnPause()
    {
        _isPaused = true;  // oyun duraklatýldý.
        //if(_coroutine != null)
        //{
        //    StopCoroutine(_coroutine); // Coroutinler her zaman direkt Method referans ile durmaz o yüzden coroutineile durdurduk
        //    _coroutine = null;
        //}
        Time.timeScale = 0f;
    }
    private void OnResume()
    {
        _isPaused = false;
        //if(_coroutine == null)
        //{
        //    _coroutine = StartCoroutine(StartTime());
        //}
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
        GameUIEvents.TimerUI += AddTime;
    }

    private void UnRegisterEvents()
    {
        GameUIEvents.GameUI -= RestartGame;
        GameUIEvents.OnPause -= OnPause;
        GameUIEvents.OnResume -= OnResume;
        GameUIEvents.TimerUI -= AddTime;
    }
}
