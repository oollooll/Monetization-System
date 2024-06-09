using UnityEngine;
using UnityEngine.Advertisements;

namespace Services.SDK.Ads
{
    public class InterstitialAd : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        [SerializeField] private string _androidAdUnitId = "Interstitial_Android";
        [SerializeField] private string _iOsAdUnitId = "Interstitial_iOS";
        string _adUnitId;
 
        void Awake()
        {
            _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
                ? _iOsAdUnitId
                : _androidAdUnitId;
            LoadAd();
        }

        public void ShowAddOneInThreeTimes()
        {
            var countPlays = PlayerPrefs.GetInt("CurrentPlayButtonPress");
            if (countPlays == 0)
            {
                ShowAd();
            }
            
            countPlays++;
           
            if (countPlays == 3)
            {
                countPlays = 0;
            }
            PlayerPrefs.SetInt("CurrentPlayButtonPress",countPlays);
            PlayerPrefs.Save();
        }
        
        public void LoadAd()
        {
            Advertisement.Load(_adUnitId, this);
        }
        
        public void ShowAd()
        {
            var s = PlayerPrefs.GetInt("NoADS");
            if (s == 1)
            {
                return;
            }
            Advertisement.Show(_adUnitId, this);
        }

        public void OnUnityAdsFailedToLoad(string _adUnitId, UnityAdsLoadError error, string message)
        {
            Debug.Log($"Error loading Ad Unit: {_adUnitId} - {error.ToString()} - {message}");
        }
 
        public void OnUnityAdsShowFailure(string _adUnitId, UnityAdsShowError error, string message)
        {
            Debug.Log($"Error showing Ad Unit {_adUnitId}: {error.ToString()} - {message}");
        }
        
        public void OnUnityAdsShowComplete(string _adUnitId, UnityAdsShowCompletionState showCompletionState)
        {
            LoadAd();
        }
 
        public void OnUnityAdsShowStart(string _adUnitId) { }
        public void OnUnityAdsShowClick(string _adUnitId) { }
        public void OnUnityAdsAdLoaded(string adUnitId) { }
    }
}
