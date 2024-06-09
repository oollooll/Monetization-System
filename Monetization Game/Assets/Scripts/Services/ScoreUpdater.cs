using System;
using TMPro;
using UnityEngine;

namespace Services
{
    public class ScoreUpdater
    {
        private TextMeshProUGUI _scoreText;
        private int _currentScore;
        private int _scoreToPass;
        private int _currentLevelIndex;
        private CoinsUpdater _coinsUpdater;

        public event Action GameWon;

        public int CurrentScore
        {
            get => _currentScore;
        }
        public int ScoreToPass
        {
            get => _scoreToPass;
        }

        
        public ScoreUpdater(CoinsUpdater coinsUpdater,TextMeshProUGUI scoreText,int currentLevelIndex,int scoreToPass)
        {
            _coinsUpdater = coinsUpdater;
            _scoreText = scoreText;
            _scoreToPass = scoreToPass;
            _currentLevelIndex = currentLevelIndex;
            _currentScore = 0;
            _scoreText.text = $"{_currentScore}/{_scoreToPass}";
        }

        public void UpdateScore()
        {
            _currentScore++;
            _coinsUpdater.UpdateScore();
            _scoreText.text = $"{_currentScore}/{_scoreToPass}";
            if (_currentScore == _scoreToPass)
            {
                GameWon?.Invoke();
            }
        }
    }
}