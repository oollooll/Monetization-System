using System;
using TMPro;
using UnityEngine;

namespace Services.ScoreDir
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _score;

        private void OnEnable()
        {
            DisplayCoins();
        }
        
        public void UpdateCoins(int amount)
        {
            SaveCoins(amount);
            DisplayCoins();
        }

        private void DisplayCoins()
        {
            var currentScore =  PlayerPrefs.GetInt("Coins");
            _score.text = $"{currentScore}x";
        }

        private void SaveCoins(int amount)
        {
            var currentScore =  PlayerPrefs.GetInt("Coins");
            currentScore += amount;
            
            PlayerPrefs.SetInt("Coins",currentScore);
            PlayerPrefs.Save();
        }
    }
}
