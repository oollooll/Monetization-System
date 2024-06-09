using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Area
{
    public class Area : MonoBehaviour
    {
        private Image _image;
        private Button _button;
        private int _indexArea;
        private int _price;
        private bool _unlocked;
        
        public string AreaName { get; private set; }

        public delegate void LockedAction(Area area);
        public event LockedAction LockedArea;

        public int Price => _price;
        public int IndexArea => _indexArea;

        public void Initialize(Sprite sprite,int index,bool unlock, int price,string name)
        {
            _indexArea = index;
            _unlocked = unlock;
            _price = price;
            AreaName = name;
            _image = GetComponent<Image>();
            _button = GetComponent<Button>();
            
            _image.sprite = sprite;
            _button.onClick.AddListener(LoadArea);
        }

        private void LoadArea()
        {
            if (_unlocked)
            {
                SceneManager.LoadSceneAsync("GamePlayScene");
                PlayerPrefs.SetInt("CurrentArea",_indexArea);
                PlayerPrefs.Save();
            }
            else
            {
                LockedArea?.Invoke(this);
            }
        }
    }
}