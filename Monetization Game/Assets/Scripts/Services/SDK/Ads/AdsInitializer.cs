using UnityEngine;
using UnityEngine.Advertisements;

namespace Services.SDK
{
    public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
    {
        [SerializeField] string _IOSGameId;
        
        bool _testMode = false;
        private string _gameId;

        public void InitializeAds()
        {
#if UNITY_IOS
            _gameId = _IOSGameId;
#elif UNITY_EDITOR
            _gameId = _IOSGameId; //Only for testing the functionality in the Editor
#endif
            if (!Advertisement.isInitialized && Advertisement.isSupported)
            {
                Advertisement.Initialize(_gameId, _testMode, this);
            }
        }
        
        public void OnInitializationComplete()
        {
            Debug.Log("Unity Ads initialization complete.");
        }
 
        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
        }
    }
}