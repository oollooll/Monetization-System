using System;
using DG.Tweening;
using Services.ScoreDir;
using UnityEngine;
using UnityEngine.UI;

namespace Services.Achivments
{
    public class Achiev:MonoBehaviour,IScoreUpdater
    {
        [SerializeField] private Button _a1Button;
        [SerializeField] private Button _a2Button;
        [SerializeField] private Button _a3Button;
        
        [SerializeField] private Image _a1Image;
        [SerializeField] private Image _a2Image;
        [SerializeField] private Image _a3Image;

        [SerializeField] private Sprite _taken;
        [SerializeField] private Sprite _ready;
        [SerializeField] private Sprite _notReady;
        
        [SerializeField] private GameObject _p1;
        [SerializeField] private GameObject _p2;
        [SerializeField] private GameObject _p3;

        private int _a1 = 10;
        private int _a2 = 2000;
        private int _a3 = 1;
        
        public event IScoreUpdater.ScoreUpdated OnScoreUpdate;

        private void Start()
        {
            var s1 = PlayerPrefs.GetInt("Ach1");
            var s3 = PlayerPrefs.GetInt("DarkStriker");
            
            var h1 = PlayerPrefs.GetInt("Achiev1");
            var h2 = PlayerPrefs.GetInt("Achiev2");
            var h3 = PlayerPrefs.GetInt("Achiev3");
            
            _a1Image.fillAmount = (float) s1 / _a1;
            _a2Image.fillAmount = (float) s1 / _a2;
            _a3Image.fillAmount = (float) s3 / _a3;

            if (_a1 > s1)
            {
                _a1Button.GetComponent<Image>().sprite = _notReady;
            }
            else if (_a1 <= s1)
            {
                if(h1 == 1)
                {
                    _a1Button.GetComponent<Image>().sprite = _taken;
                    _p1.SetActive(false);
                }
                else
                {
                    _a1Button.GetComponent<Image>().sprite = _ready;
                    _a1Button.onClick.AddListener(() => Achive(200,_a1Button,_p1,1));
                }
            }

            if (s1 < _a2)
            {
                _a2Button.GetComponent<Image>().sprite = _notReady;
            }
            else if (s1 >= _a2)
            {
                if(h2 == 1)
                {
                    _a2Button.GetComponent<Image>().sprite = _taken;
                    _p2.SetActive(false);
                    
                }
                else
                {
                    _a2Button.GetComponent<Image>().sprite = _ready;
                    _a2Button.onClick.AddListener(() => Achive(300,_a2Button,_p2,2));
                }
            }
            
            if (s3 < _a3)
            {
                _a3Button.GetComponent<Image>().sprite = _notReady;
            }
            else if (s3 == _a3)
            {
                if(h3 == 1)
                {
                    _a3Button.GetComponent<Image>().sprite = _taken;
                    _p3.SetActive(false);
                }
                else
                {
                    _a3Button.GetComponent<Image>().sprite = _ready;
                    _a3Button.onClick.AddListener(() => Achive(500,_a3Button,_p3,3));
                }
            }
        }

        private void Achive(int amount,Button button,GameObject gameObject,int achive)
        {
            PlayerPrefs.SetInt($"Achiev{achive}",1);
            PlayerPrefs.Save();
            gameObject.SetActive(false);
            OnScoreUpdate?.Invoke(amount);
            button.onClick.RemoveAllListeners();
            button.GetComponent<Image>().sprite = _taken;
        }
    }
}