using System;
using Services.ScoreDir;
using UnityEngine;
using UnityEngine.UI;


namespace MainMenu.DailyBonusDir
{
    public class DailyBonus : MonoBehaviour,IScoreUpdater
    {
        [SerializeField] private Button _claim;
        public event Action StartTimer;
        public event IScoreUpdater.ScoreUpdated OnScoreUpdate;
        
        private Image _bonusImage;
        private Image _claimButtonImage;
        private int _prize;
        private int _indexOfDay;

        public void Initialize(Sprite currentState,int prize,int index)
        {
            _bonusImage = GetComponent<Image>();
            _bonusImage.sprite = currentState;
            _prize = prize;
            _indexOfDay = index;
        }

        public void ActivateClaimButton(Sprite currentStat)
        {
            _claim.gameObject.SetActive(true);
            _claim.onClick.AddListener(() => ClaimReward(currentStat));
        }

        private void ClaimReward(Sprite claimed)
        {
            _bonusImage.sprite = claimed;
            _claim.gameObject.SetActive(false);
            _claim.onClick.RemoveAllListeners();
            
            
            if (_indexOfDay == 6)
            {
                _indexOfDay = 0;
                PlayerPrefs.SetInt("BluePlane",1);
                PlayerPrefs.Save();
            }
            else
            {
                _indexOfDay++;
                OnScoreUpdate?.Invoke(_indexOfDay * 100);
            }
            
            PlayerPrefs.SetInt("CurrentDayToBonus",_indexOfDay);
            PlayerPrefs.Save();

            SaveLastTime();
            StartTimer?.Invoke();
        }
        
        public void SaveLastTime()
        {
            DateTime currentTime = DateTime.Now;
            PlayerPrefs.SetString("SavedTimeDailyBonus", currentTime.ToString());
            PlayerPrefs.Save();
        }

       
    }
}