using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Services.ScrolledPanelDir
{
    public class ScrolledPanel:MonoBehaviour
    {
        public delegate void ScrollByButton(ScrolledPanel panel);
        [field:SerializeField] public float PanelNormalizedPosition { get; private set; }
        [SerializeField] public Button _panelSelectButton;
        
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private Sprite _acctiveImage;
        [SerializeField] private Sprite _unAcctiveImage;

        private Image _buttonImage;
        
        public void Initialize(ScrollByButton scrollByButton)
        {
            _buttonImage = _panelSelectButton.GetComponent<Image>();
            _panelSelectButton.onClick.AddListener(() => scrollByButton(this));
        }

        public void ExitPanel()
        {
            SetButtonUnActive();
        }
        
        public void EnterPanel()
        {
            SetButtonActive();
        }

        public void DisableRect()
        {
            if(PanelNormalizedPosition == 1)
                return;
            _scrollRect.enabled = false;
        }
        
        public void EnableRect()
        {
            if(PanelNormalizedPosition == 1)
                return;
            _scrollRect.enabled = true;
        }
        
        private void SetButtonActive()
        {
            _buttonImage.DOFade(0.3f, 0.5f).OnComplete(()=>
            {
                _buttonImage.sprite = _acctiveImage;
                _buttonImage.DOFade(1, 0.5f);
            });;
        }
        
        private void SetButtonUnActive()
        {
            _buttonImage.DOFade(0.3f, 0.5f).OnComplete(()=>
            {
                _buttonImage.sprite = _unAcctiveImage;
                _buttonImage.DOFade(1f, 0.5f);
                ScrollToTop();
            });
        }
        
        private void ScrollToTop()
        {
            if(PanelNormalizedPosition == 1)
                return;
            _scrollRect.verticalNormalizedPosition = 1;
        }
    }
}