using System;
using System.Collections;
using System.Collections.Generic;
using Services.ScoreDir;
using TMPro;
using UnityEngine;

namespace MainMenu.DailyBonusDir
{
    public class DailyBonusController : MonoBehaviour,IScoreUpdater
    {
        [SerializeField] private List<DailyBonus> _dailyBonusList;
        [SerializeField] private Sprite _claimed;
        [SerializeField] private Sprite _readyToClaim;
        [SerializeField] private Sprite _soon;
        [SerializeField] private Sprite _claimedF;
        [SerializeField] private Sprite _readyToClaimF;
        [SerializeField] private Sprite _soonF;
        [SerializeField] private TextMeshProUGUI _timerText;

        public event IScoreUpdater.ScoreUpdated OnScoreUpdate;

        private void Initialization()
        {
            var currentDayIndex = PlayerPrefs.GetInt("CurrentDayToBonus");
            Sprite currentsprite;

            for (int i = 0; i < _dailyBonusList.Count - 1; i++)
            {
                if (i < currentDayIndex)
                {
                    currentsprite = _claimed;
                }
                else if(i == currentDayIndex)
                {
                    if (CanBeClaimed())
                    {
                        currentsprite = _readyToClaim;
                        _dailyBonusList[i].ActivateClaimButton(_claimed);
                    }
                    else
                    {
                        currentsprite = _soon;
                    }
                }
                else
                {
                    currentsprite = _soon;
                }
                _dailyBonusList[i].Initialize(currentsprite,50,i);
                _dailyBonusList[i].StartTimer += StartTimer;
                _dailyBonusList[i].OnScoreUpdate += OnScoreUpdate;
            }
            
            if  (currentDayIndex < 6)
            {
                currentsprite = _soonF;
            }
            else
            {
                currentsprite = _readyToClaimF;
                if (CanBeClaimed())
                {
                    _dailyBonusList[6].ActivateClaimButton(_claimedF);
                }
            }
            
            _dailyBonusList[6].Initialize(currentsprite,200,6);
            _dailyBonusList[6].StartTimer += StartTimer;
        }

        private void OnEnable()
        {
            if (CanBeClaimed())
            {
                _timerText.text = "Your gift already waiting!";
            }
            else
            {
                StartTimer();
            }
            
            Initialization();
        }

        private void OnDisable()
        {
            foreach (var bonus in _dailyBonusList)
            {
                bonus.StartTimer -= StartTimer;
                bonus.OnScoreUpdate -= OnScoreUpdate;
            }
        }

        private void StartTimer()
        {
            StartCoroutine(DailyBonusTimeLeft());
        }

        private IEnumerator DailyBonusTimeLeft()
        {
            string savedTimeAsString = PlayerPrefs.GetString("SavedTimeDailyBonus");
            var lastTime = DateTime.Parse(savedTimeAsString);
            var nextClaimTime = lastTime.AddHours(24);
            while (DateTime.Now <= nextClaimTime)
            {
                var timer = nextClaimTime - DateTime.Now;
            
                yield return new WaitForSecondsRealtime(1f);
            
                string timerString = string.Format("{0:00}:{1:00}:{2:00}", timer.Hours, timer.Minutes, timer.Seconds);
                _timerText.text = timerString;
            }
            
            _timerText.text = "Your gift already waiting!";
            foreach (var bonus in _dailyBonusList)
            {
                bonus.StartTimer -= StartTimer;
                bonus.OnScoreUpdate -= OnScoreUpdate;
            }
            Initialization();
        }

        private bool CanBeClaimed()
        {
            if (PlayerPrefs.HasKey("SavedTimeDailyBonus"))
            {
                string savedTimeAsString = PlayerPrefs.GetString("SavedTimeDailyBonus");
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
    }
}