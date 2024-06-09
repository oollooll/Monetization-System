using TMPro;
using UnityEngine;

namespace Services
{
    public class CoinsUpdater
    {
        private TextMeshProUGUI _coins;
        private int _currentScore;
        private int _levelModif;
        private int _currentGameScore;

        public int CurrentGameScore
        {
            get => _currentGameScore;
        }
        public CoinsUpdater(TextMeshProUGUI scoreText,int levelModif)
        {
            _coins = scoreText;
            _levelModif = levelModif;
            _currentScore =  PlayerPrefs.GetInt("Coins");
            _currentGameScore = 0;
            _coins.text = $"{_currentScore}x";
        }

        public void UpdateScore()
        {
            _currentGameScore += 1 * _levelModif;
            _currentScore += 1 * _levelModif;
            _coins.text = $"{_currentScore}x";
        }

        public void SaveCoins()
        {
            PlayerPrefs.SetInt("Coins",_currentScore);
            PlayerPrefs.Save();
        }
    }
}