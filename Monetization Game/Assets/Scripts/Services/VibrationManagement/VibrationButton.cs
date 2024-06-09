using System;
using UnityEngine;
using UnityEngine.UI;

namespace Services.VibrationManagement
{
    public class VibrationButton : MonoBehaviour
    {
        [SerializeField] private Sprite _activeVibration;
        [SerializeField] private Sprite _nonActiveVibration;

        private Button _vibroButton;
        private Image _vibroImage;

        private void Awake()
        {
            SetVibration();
            var soundOn = PlayerPrefs.GetInt("VibroOn");
        
            _vibroButton = GetComponent<Button>();
            _vibroImage = GetComponent<Image>();
            _vibroButton.onClick.AddListener(SwitchSound);
            _vibroImage.sprite = soundOn == 0 ? _nonActiveVibration : _activeVibration;
        }
        
        private void SetVibration()
        {
            if (!PlayerPrefs.HasKey("SoundOn"))
            {
                PlayerPrefs.SetInt("SoundOn", 1);
            }
        }

        private void SwitchSound()
        {
            _vibroImage.sprite = _vibroImage.sprite == _activeVibration ? _nonActiveVibration : _activeVibration;
        
            var soundOn = PlayerPrefs.GetInt("VibroOn");
            if (soundOn == 0)
            {
                soundOn = 1;
            }
            else
            {
                soundOn = 0;
            }

            PlayerPrefs.SetInt("VibroOn",soundOn);
            PlayerPrefs.Save();
        }
    }
}