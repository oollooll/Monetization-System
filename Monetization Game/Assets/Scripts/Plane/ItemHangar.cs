using System;
using UnityEngine;
using UnityEngine.UI;

namespace Plane
{
    public class ItemHangar:MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private GameObject _price;

        public int Index { get; private set; }
        public int Price { get; private set; }
        public string Name { get; private set; }
        
        public delegate void PlaneButtonPress(ItemHangar item);
        public event PlaneButtonPress UnlockPlane;
        public event PlaneButtonPress SelectPlane;

        private Image _image;


        public void Initialize(int index,int price,string name,Sprite sprite)
        {
            Index = index;
            Price = price;
            Name = name;
            _image = _button.GetComponent<Image>();
            InitializeButton(sprite);
        }

        public void InitializeButton(Sprite sprite)
        {
            _button.onClick.RemoveAllListeners();
            var status = PlayerPrefs.GetInt(Name);

            if (status == 0)
            {
                _button.onClick.AddListener(Buy);
                _price.SetActive(true);
            }
            else
            {
                _image.sprite = sprite;
                _button.onClick.AddListener(Select);
                _price.SetActive(false);
            }
        }

        public void ChangeButtonSprite(Sprite sprite)
        {
            _image.sprite = sprite;
        }

        private void Buy()
        {
            UnlockPlane?.Invoke(this);
        }
        
        private void Select()
        {
            SelectPlane?.Invoke(this);
        }
    }
}