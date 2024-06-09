using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Plane
{
    public class SkillController:MonoBehaviour
    {
        [SerializeField] private Button _enemyCrashButton;
        [SerializeField] private Sprite _lockState;
        [SerializeField] private Sprite _unLockState;
        [SerializeField] private Sprite _activeState;

        [SerializeField] private Button _enemySlowButton;
        [SerializeField] private Sprite _lockStateSlow;
        [SerializeField] private Sprite _unLockStateSlow;
        [SerializeField] private Sprite _activeStateSlow;

        [SerializeField] private GameObject _crash;

        private bool _isSkillActive;
        private Image _enemyCrashImage;
        private Image _enemySlowImage;
        
        public void Initialize()
        {
            _crash.SetActive(false);
            _enemyCrashImage = _enemyCrashButton.GetComponent<Image>();
            _enemySlowImage = _enemySlowButton.GetComponent<Image>();

            DisplaySkills();
        }

        private void DisplaySkills()
        {
            var count1 = PlayerPrefs.GetInt("EnemyCrash");
            var count2 = PlayerPrefs.GetInt("SlowFly");
            
            if (count1 > 0)
            {
                _enemyCrashImage.sprite = _unLockState;
                _enemyCrashButton.onClick.AddListener(Crash);
            }
            else
            {
                _enemyCrashButton.onClick.RemoveAllListeners();
                _enemyCrashImage.sprite = _lockState;
            }
            
            if (count2 > 0)
            {
                _enemySlowImage.sprite = _unLockStateSlow;
                _enemySlowButton.onClick.AddListener(Slow);
            }
            else
            {
                _enemySlowButton.onClick.RemoveAllListeners();
                _enemySlowImage.sprite = _lockStateSlow;
            }
        }
        
        private void Crash()
        {
            if(_isSkillActive)
                return;
            
            _isSkillActive = true;
            
            var count1 = PlayerPrefs.GetInt("EnemyCrash");
            PlayerPrefs.SetInt("EnemyCrash",count1 - 1);
            PlayerPrefs.Save();
           
            _enemyCrashImage.sprite = _activeState;
            
            var t = 0;
            _crash.SetActive(true);
            DOTween.To(() => 
                t, x => t = x, 1, 10f).OnComplete(() =>
            {
                DisplaySkills();
                _crash.SetActive(false);
                _isSkillActive = false;
            });
        }
        
        private void Slow()
        {
            if(_isSkillActive)
                return;

            _isSkillActive = true;
            
            var count1 = PlayerPrefs.GetInt("SlowFly");
            PlayerPrefs.SetInt("SlowFly",count1 - 1);
            PlayerPrefs.Save();
            
            _enemySlowImage.sprite = _activeStateSlow;
            
            var t = 0;
            Time.timeScale = 0.5f;
            DOTween.To(() => 
                t, x => t = x, 1, 5f).OnComplete(() =>
            {
                DisplaySkills();
                Time.timeScale = 1f;
                _isSkillActive = false;
            });
        }
    }
}