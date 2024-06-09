using System;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using Services.ScoreDir;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

namespace Shop
{
    public class Shop : MonoBehaviour,IScoreUpdater,IStoreListener
    {
        [SerializeField] private List<ShopItem> _shopItems;
        [SerializeField] private Image _notEnough;
        [SerializeField] private Button _restorePurchase;
        
        [SerializeField] private Button _button2;
        [SerializeField] private TextMeshProUGUI _text2;
        [SerializeField] private List<TextMeshProUGUI> _prices;

        private bool _isAnimPlay;
        private static IStoreController storeController;
        private static IExtensionProvider extensionProvider;
        public event IScoreUpdater.ScoreUpdated OnScoreUpdate;
        
        public void Initialize()
        {
            InitializePurchasing();
            var productCollection = storeController.products;
            var currentScore =  PlayerPrefs.GetInt("Coins");
            foreach (var item in _shopItems)
            {
                item.Initialize();
                item.OnItemBuy += BuyItem;
                
                if (item.ItemPrice > currentScore)
                {
                    item.BuyButton.interactable = false;
                    NotEnoughAnim();
                }
                else
                {
                    item.BuyButton.interactable = true;
                }
            }

            for (int i = 0; i < _prices.Count; i++)
            {
                _prices[i].text = productCollection.all[i].metadata.localizedDescription;
            }
            
            _restorePurchase.gameObject.SetActive(true);
            _restorePurchase.onClick.AddListener(NoAds);
            _notEnough.gameObject.SetActive(false);
            RealMoneyShopStart();
        }
        
        private void InitializePurchasing()
        {
            if (IsInitialized())
            {
                return;
            }

            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            UnityPurchasing.Initialize(this, builder);
        }
        
        private bool IsInitialized()
        {
            return storeController != null && extensionProvider != null;
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            throw new NotImplementedException();
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            throw new NotImplementedException();
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            throw new NotImplementedException();
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            throw new NotImplementedException();
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            storeController = controller;
            extensionProvider = extensions;
        }

        private void RedisplayShop()
        {
            var currentScore =  PlayerPrefs.GetInt("Coins");
            
            foreach (var item in _shopItems)
            {
                if (item.ItemPrice > currentScore)
                {
                    item.BuyButton.interactable = false;
                    NotEnoughAnim();
                }
                else
                {
                    item.BuyButton.interactable = true;
                }
            }
        }
        
        private void BuyItem(ShopItem item)
        {
            var currentScore =  PlayerPrefs.GetInt("Coins");

            if (currentScore < item.ItemPrice)
            {
                NotEnoughAnim();
            }
            else
            {
                var count = PlayerPrefs.GetInt($"{item.ItemName}");

                PlayerPrefs.SetInt($"{item.ItemName}", count + 1);
                PlayerPrefs.Save();

                OnScoreUpdate?.Invoke(-item.ItemPrice);
                RedisplayShop();
            }
        }

        private void NotEnoughAnim()
        {
            if(_isAnimPlay)
                return;

            _isAnimPlay = true;
            _restorePurchase.gameObject.SetActive(false);
            _notEnough.gameObject.SetActive(true);
            var s = DOTween.Sequence();
            s.Append(_notEnough.DOFade(1,1));
            
            for (int i = 0; i < 4; i++)
            {
                s.Append(_notEnough.DOFade(0.6f,0.5f));
                s.Append(_notEnough.DOFade(1f,0.5f));
            }
            s.Append(_notEnough.DOFade(0,1)).OnComplete(()=> OnAnimEnded());
        }

        private void OnAnimEnded()
        {
            _isAnimPlay = false;
            _restorePurchase.gameObject.SetActive(true);
            _notEnough.gameObject.SetActive(false);
        }

        private void RealMoneyShopStart()
        {
            var ads = PlayerPrefs.GetInt("NoADS");

            if (ads == 1)
            {
                _button2.interactable = false;
                _text2.text = "bought";
            }
        }

        public void OnPurchaseCompleted(Product product)
        {
            if (product.definition.id == "com.bradley.apexcoins.5k")
            {
                BuyCoins(1000);
            }
            else if (product.definition.id == "com.bradley.apexcoins.12k")
            {
                BuyCoins(3500);
            }
            else if (product.definition.id == "com.bradley.apexcoins.35k")
            {
                BuyCoins(7500);
            }
        }
        

        private void NoAds()
        {
            _button2.interactable = false;
            _text2.text = "bought";
            PlayerPrefs.SetInt("NoADS",1);
            PlayerPrefs.Save();
        }
    
        private void BuyCoins(int amount)
        {
            OnScoreUpdate?.Invoke(amount);
            RedisplayShop();
        }

        public void CheckNonConsumable(bool succes, string? error)
        {
            var productCollection = storeController.products;
            foreach (var iap in productCollection.all)
            {
                if (iap.hasReceipt)
                {
                    OnPurchaseCompleted(iap);
                }  
            }
        }
        
    }
}
