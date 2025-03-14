using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private int _targetScore;
    private int score = 0;
    public int Score => score;
    public int TargetScore => _targetScore;
    public TextMeshProUGUI ScoreText => _scoreText;
    private int _highScore;
    public int HighScore => _highScore;

    private void Start()
    {
        _highScore = PlayerPrefs.GetInt("HighScore", 0);
       ResetScore();
    }
    public void ResetScore()
    {
        score = 0;
        UpdateScore();
    }
    private void  UpdateScore()
    {
        if(_scoreText != null)
        {
            _scoreText.text =  "Score:" + score.ToString();
        }
    }

    public void SaveHighScore()
    {
        if(score > _highScore)
        {
            _highScore = score;
            PlayerPrefs.SetInt("HighScore",_highScore);
        }
        PlayerPrefs.Save();
    }
  
    private void IncreaseScore(int amount)
    {
        score += amount;
        UpdateScore();
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
        ScoreEvents.GameScoreEvents += IncreaseScore;
    }

    private void UnRegisterEvents() 
    {
        ScoreEvents.GameScoreEvents -= IncreaseScore;
    }
}
