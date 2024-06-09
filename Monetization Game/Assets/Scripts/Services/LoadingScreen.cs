using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Services
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private Image _loadingBar;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private GameObject _loadingPanel;
        
        private AsyncOperation _loadingScreneOperation;
        private static bool _isGameLoaded;

        private void Awake()
        {
            _loadingPanel.SetActive(false);
            if (!_isGameLoaded)
            {
                GameLoadingAnimation();
                _loadingPanel.SetActive(true);
            }
        }

        private void Update()
        {
            if (!_isGameLoaded)
            {
                var per = (int)(_loadingBar.fillAmount * 100);
                _text.text = $"{per}%";
            }
        }

        public void GameLoadingAnimation()
        {
            if (_isGameLoaded)
                return;
            
            DOTween.To(() => 
                _loadingBar.fillAmount, x => _loadingBar.fillAmount = x, 1, 5f).OnComplete(LoadingEnded);
        }

        public void LoadingEnded()
        {
            _loadingPanel.SetActive(false);
            _isGameLoaded = true;
        }
    }
}