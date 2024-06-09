using Services.ScoreDir;
using Services.TimeDir;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

namespace Services.SDK.Ads
{
    public class RewardedAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener,IScoreUpdater
    {
        [SerializeField] Button _showAdButton;
        [SerializeField] private Timer _timer;
        [SerializeField] string _androidAdUnitId = "Rewarded_Android";
        [SerializeField] string _iOSAdUnitId = "Rewarded_iOS";
        string _adUnitId = null; // This will remain null for unsupported platforms
        public event IScoreUpdater.ScoreUpdated OnScoreUpdate;
 
        void Awake()
        {
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
            _adUnitId = _androidAdUnitId;
#endif
            LoadAd();
        }
        
        public void LoadAd()
        {
            Debug.Log("Loading Ad: " + _adUnitId);
            Advertisement.Load(_adUnitId, this);
        }
        
        public void OnUnityAdsAdLoaded(string adUnitId)
        {
            Debug.Log("Ad Loaded: " + adUnitId);
        }
 
        public void ShowAd()
        {
            var s = PlayerPrefs.GetInt("NoADS");
            if (s == 1)
            {
                return;
            }
            LoadAd();
            
            Advertisement.Show(_adUnitId, this);
            _showAdButton.interactable = false;
            _timer.StartTimer();
          
            // Then show the ad:
            
        }
        
        public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
        {
            if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                OnScoreUpdate?.Invoke(1000);
                LoadAd();
                
            }
        }
        
        public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message) { }
        public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message) { }
        public void OnUnityAdsShowStart(string adUnitId) { }
        public void OnUnityAdsShowClick(string adUnitId) { }
 
        void OnDestroy()
        {
            // Clean up the button listeners:
            _showAdButton.onClick.RemoveAllListeners();
        }

       
    }
}