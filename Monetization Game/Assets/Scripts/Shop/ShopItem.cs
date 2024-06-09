using System;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class ShopItem : MonoBehaviour
    {
        [field:SerializeField] public Button BuyButton { get; private set; }
        [field:SerializeField] public int ItemPrice { get; private set; }
        [field:SerializeField] public string ItemName { get; private set; }


        public delegate void ItemBuy(ShopItem item);
        public event ItemBuy OnItemBuy;

        public void Initialize()
        {
            BuyButton.onClick.AddListener(Buy);
        }
        private void Buy()
        {
            OnItemBuy?.Invoke(this);
        }
    }
}