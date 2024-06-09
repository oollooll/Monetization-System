using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Services.TimeDir
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] Button _showAdButton;
        [SerializeField] private TextMeshProUGUI _timerText;
        
        private TimeController _timeController;
        private void Awake()
        {
            _timeController = new TimeController();
        }
        
        private void OnEnable()
        {
            if (CanBeClaimed())
            {
                _timerText.gameObject.SetActive(false);
                _showAdButton.interactable = true;
            }
            else
            {
                StartCoroutine(UpdateTimer());
                _timerText.gameObject.SetActive(true);
            }
        }

        public void StartTimer()
        {
            _timeController.SaveUnlockTime();
            StartCoroutine(UpdateTimer());
            _timerText.gameObject.SetActive(true);
        }
        
        private bool CanBeClaimed()
        {
            if (PlayerPrefs.HasKey("SavedTime"))
            {
                string savedTimeAsString = PlayerPrefs.GetString("SavedTime");
                var lastTime = DateTime.Parse(savedTimeAsString);

                var nextClaimTime = lastTime.AddHours(24);

                if (DateTime.Now > nextClaimTime)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        private IEnumerator UpdateTimer()
        {
            string savedTimeAsString = PlayerPrefs.GetString("SavedTime");
            var lastTime = DateTime.Parse(savedTimeAsString);
            var nextClaimTime = lastTime.AddHours(24);
            while (DateTime.Now <= nextClaimTime)
            {
                var timer = nextClaimTime - DateTime.Now;
            
                yield return new WaitForSecondsRealtime(1f);
            
                string timerString = string.Format("{0:00}:{1:00}:{2:00}", timer.Hours, timer.Minutes, timer.Seconds);
                _timerText.text = timerString;
            }

            _timerText.gameObject.SetActive(false);
            _showAdButton.interactable = true;
        }
    }
}