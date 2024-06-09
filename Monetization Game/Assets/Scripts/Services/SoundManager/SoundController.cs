using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Services.SoundManager
{
    public class SoundController : MonoBehaviour
    {
        [SerializeField] private Sprite _soundOnSprite;
        [SerializeField] private Sprite _soundOffSprite;
        [SerializeField] private Button _soundButton;
        [SerializeField] private Button _musicButton;
        private Image _soundImage;
        private Image _musicImage;

        public event Action MusicOff;
        public event Action MusicOn;

        private void Awake()
        {
            SetSound();
            var soundOn = PlayerPrefs.GetInt("SoundOn");
            var musicOn = PlayerPrefs.GetInt("MusicOn");
            
            _soundImage = _soundButton.GetComponent<Image>();
            _musicImage = _musicButton.GetComponent<Image>();
            
            _soundButton.onClick.AddListener(SwitchSound);
            _musicButton.onClick.AddListener(SwitchMusic);
            
            _soundImage.sprite = soundOn == 0 ? _soundOffSprite : _soundOnSprite;
            _musicImage.sprite = musicOn == 0 ? _soundOffSprite : _soundOnSprite;
        }
        
        private void SetSound()
        {
            if (!PlayerPrefs.HasKey("SoundOn"))
            {
                PlayerPrefs.SetInt("SoundOn", 1);
            }
        }
        
        
        private void SwitchMusic()
        {
            _musicImage.sprite = _musicImage.sprite == _soundOnSprite ? _soundOffSprite : _soundOnSprite;
        
            var soundOn = PlayerPrefs.GetInt("MusicOn");
            if (soundOn == 0)
            {
                soundOn = 1;
                MusicOn?.Invoke();
            }
            else
            {
                soundOn = 0;
                MusicOff?.Invoke();
            }
           
        
            PlayerPrefs.SetInt("MusicOn",soundOn);
            PlayerPrefs.Save();
        }

        private void SwitchSound()
        {
            _soundImage.sprite = _soundImage.sprite == _soundOnSprite ? _soundOffSprite : _soundOnSprite;
        
            var soundOn = PlayerPrefs.GetInt("SoundOn");
            if (soundOn == 0)
            {
                soundOn = 1;
            }
            else
            {
                soundOn = 0;
            }
           
        
            PlayerPrefs.SetInt("SoundOn",soundOn);
            PlayerPrefs.Save();
        }
    }
}
