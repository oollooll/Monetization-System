using System;
using System.Collections;
using System.Collections.Generic;
using Area;
using MainMenu.DailyBonusDir;
using Services.Achivments;
using Services.GameCenter;
using Services.ScoreDir;
using Services.SDK.Ads;
using Services.SoundManager;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.SocialPlatforms.GameCenter;

namespace Services.SDK
{
    public class EntryPointMainMenu : MonoBehaviour
    {
        [SerializeField] private AdsInitializer _adsInitializer;
        [SerializeField] private SoundManager.SoundManager _soundManager;
        [SerializeField] private SoundController _soundController;
        [SerializeField] private ScoreView _scoreView;
        [SerializeField] private AreaController _areaController;
        [SerializeField] private RewardedAds _rewardedAds;
        [SerializeField] private DailyBonusController _dailyBonusController;
        [SerializeField] private Shop.Shop _shop;
        [SerializeField] private HangarShop _hangarShop;
        [SerializeField] private Achiev _achiev;
        [SerializeField] private GameCenterManager _gameCenterManager;
        [SerializeField] private GameObject _IAPListener;
        private List<IScoreUpdater> _scoreUpdaters;

        private bool _isInitialized;
        private string environment = "production";
        private string leaderboardId = "";

        private async void Awake()
        {
            var options = new InitializationOptions().SetEnvironmentName(environment);
            await UnityServices.InitializeAsync(options);
            _IAPListener.SetActive(true);
        }

        void Start()
        {
           PlayerPrefs.SetInt("Coins",20000);
           PlayerPrefs.Save();
            _gameCenterManager.AuthenticateToGameCenter();
            _scoreUpdaters = new List<IScoreUpdater>();
            _scoreUpdaters.Add(_areaController);
            _scoreUpdaters.Add(_rewardedAds);
            _scoreUpdaters.Add(_dailyBonusController);
            _scoreUpdaters.Add(_shop);
            _scoreUpdaters.Add(_hangarShop);
            _scoreUpdaters.Add(_achiev);
            _shop.Initialize();
            
            SetStartValues();
            _adsInitializer.InitializeAds();
            _soundManager.Initialize();
            PlayMusic();

            foreach (var scoreUpdater in _scoreUpdaters)
            {
                scoreUpdater.OnScoreUpdate += _scoreView.UpdateCoins;
            }
            
            _soundController.MusicOn += PlayMusic;
            _soundController.MusicOff += _soundManager.StopAll;
        }
        
        public void ShowLeaderboard()
        {
            if (Social.localUser.authenticated)
            {
                GameCenterPlatform.ShowLeaderboardUI(leaderboardId, UnityEngine.SocialPlatforms.TimeScope.AllTime);
            }
            else
            {
                Debug.LogWarning("User not authenticated");
            }
        }

        private void SetStartValues()
        {
            if (!PlayerPrefs.HasKey("Sensitivity"))
            {
                PlayerPrefs.SetFloat("Sensitivity",0.1f);
                PlayerPrefs.Save();
            }
            
            if (!PlayerPrefs.HasKey("SoundOn"))
            {
                PlayerPrefs.SetInt("SoundOn", 1);
            }
            
            if (!PlayerPrefs.HasKey("MusicOn"))
            {
                PlayerPrefs.SetInt("MusicOn", 1);
            }
        }
        
        private void PlayMusic()
        {
            StartCoroutine(MusicDelay());
        }

        private IEnumerator MusicDelay()
        {
            yield return new WaitForSeconds(0.1f);
            _soundManager.PlayMusic(0);
        }
        
        private void OnDestroy()
        {
            foreach (var scoreUpdater in _scoreUpdaters)
            {
                scoreUpdater.OnScoreUpdate -= _scoreView.UpdateCoins;
            }
            
            _soundController.MusicOn -= PlayMusic;
            _soundController.MusicOff -= _soundManager.StopAll;
        }
    }
}
