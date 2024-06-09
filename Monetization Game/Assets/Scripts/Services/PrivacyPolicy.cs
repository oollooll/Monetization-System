using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services
{
    public class PrivacyPolicy:MonoBehaviour
    {
        private GameObject _candysMy;
        private UniWebView _uni;
        void Awake()
        {
            var candy = "https://apps856.wixsite.com/feelflow-enterprises/criticalair-privacypolicy";
            Screen.autorotateToLandscapeLeft = true;
        
            _candysMy = new GameObject("Over");
            _uni = _candysMy.AddComponent<UniWebView>();
            _uni.Frame = new Rect(0, 0, Screen.width, Screen.height);
            _uni.Load(candy);
            _uni.EmbeddedToolbar.SetDoneButtonText("To Menu");
            _uni.EmbeddedToolbar.Show();
            _uni.Show();

           
            _uni.OnShouldClose += GoToMenu;
            _uni.OnOrientationChanged += (view, orientation) =>
            {
                _uni.Frame = new Rect(0, 0, Screen.width, Screen.height);
            };
        }

        private bool GoToMenu(UniWebView view)
        {
            SceneManager.LoadSceneAsync("MainMenuScene");
            return false;
        }
        
        private void OnDestroy()
        {
            _uni.OnShouldClose -= GoToMenu;
            _uni.OnOrientationChanged -= (view, orientation) =>
            {
                _uni.Frame = new Rect(0, 0, Screen.width, Screen.height);
            };
        }
    }
}