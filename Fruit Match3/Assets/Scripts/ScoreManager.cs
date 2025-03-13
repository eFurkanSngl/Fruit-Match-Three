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

    private void Start()
    {
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
