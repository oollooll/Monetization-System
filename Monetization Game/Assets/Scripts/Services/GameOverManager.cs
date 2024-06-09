using System;
using System.Collections;
using DG.Tweening;
using Services.GameCenter;
using Services.SDK.Ads;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services
{
    public class GameOverManager : MonoBehaviour
    {
        [SerializeField] private GameObject _winPanel;
        [SerializeField] private TextMeshProUGUI _scoreWin;
        [SerializeField] private TextMeshProUGUI _scoreLoss;
        [SerializeField] private GameObject _lossPanel;
        
        private InterstitialAd _interstitialAd;
        private int _indexLevel;
        private ScoreUpdater _scoreUpdater;
        private CoinsUpdater _coinsUpdater;
        private ProjectUpdater.ProjectUpdater _projectUpdater;
        private GameCenterManager _gameCenterManager;
        public bool IsGameOver { get; private set; }

        public void Initialize(CoinsUpdater coinsUpdater,ScoreUpdater scoreUpdater, int indexLevel,ProjectUpdater.ProjectUpdater projectUpdater, InterstitialAd interstitialAd,GameCenterManager gameCenterManager)
        {
            _coinsUpdater = coinsUpdater;
            _scoreUpdater = scoreUpdater;
            _indexLevel = indexLevel;
            _projectUpdater = projectUpdater;
            _interstitialAd = interstitialAd;
            _gameCenterManager = gameCenterManager;
        }
        
        public void ShowWinPAnel()
        {
            _winPanel.SetActive(true);
            GameOver();
            _scoreWin.text = $"{_coinsUpdater.CurrentGameScore}x";
        }

        private void ShowAdd()
        {
            var countGameOvers = PlayerPrefs.GetInt("CurrentGameOverCount");
            countGameOvers++;
            
            if (countGameOvers == 3)
            {
                _interstitialAd.ShowAd();
                countGameOvers = 0;
            }
            PlayerPrefs.SetInt("CurrentGameOverCount",countGameOvers);
            PlayerPrefs.Save();
        }
        
        public void ShowLossPAnel()
        {
            _lossPanel.SetActive(true);
            _scoreLoss.text = $"{_scoreUpdater.CurrentScore}/{_scoreUpdater.ScoreToPass}";
            GameOver();
        }

        private void GameOver()
        {
            _projectUpdater.IsPaused = true;
            IsGameOver = true;
            _coinsUpdater.SaveCoins();
            SaveScore();
            ShowAdd();
        }

        private void SaveScore()
        {
            var s1 = PlayerPrefs.GetInt("Ach1");
            PlayerPrefs.SetInt("Ach1",s1 + _scoreUpdater.CurrentScore);
            PlayerPrefs.Save();
            _gameCenterManager.ReportScore(s1 + _scoreUpdater.CurrentScore);
        }

        public void RestartGame()
        {
            SceneManager.LoadSceneAsync("GamePlayScene");
        }
        
        public void ToMenu()
        {
            SceneManager.LoadSceneAsync("MainMenuScene");
            _projectUpdater.IsPaused = false;
        }

        private void OnDestroy()
        {
            DOTween.KillAll();
        }
    }
}