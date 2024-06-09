using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Services;
using Services.ScrolledPanelDir;
using UnityEngine;
using UnityEngine.UI;

public class MainSwitchPanel : MonoBehaviour
{
    [SerializeField] private SwipeInput _swipeInput;
    [SerializeField] private ScrollRect _horizontalScrollRect;
    [SerializeField] private List<ScrolledPanel> _verticalScrolledPanels;
    
    private ScrolledPanel _currentScrolledPanel;
    private ScrolledPanel _previousScrolledPanel;
    private bool _needToSwitch;
    private bool _switchByButtons;

    private void Start()
    {
        foreach (var verticalScroll in _verticalScrolledPanels)
        {
            verticalScroll.Initialize(ScrollByButtons);

            if (verticalScroll.PanelNormalizedPosition == 0.5f)
            {
                _horizontalScrollRect.horizontalNormalizedPosition = verticalScroll.PanelNormalizedPosition;
                _currentScrolledPanel = verticalScroll;
            }
        }
        _previousScrolledPanel = _currentScrolledPanel;
        
        _swipeInput.TouchEnded += EndScroll;
        _swipeInput.HorizontalSwipe += ChangeScrollBehaviour;
        _swipeInput.VerticalSwipe += ChangeScrollBehaviourBak;
    }

    public void DisableMainSwitch()
    {
        _horizontalScrollRect.enabled = false;
    }
    
    public void EnableMainSwitch()
    {
        _horizontalScrollRect.enabled = true;
    }

    private void ChangeScrollBehaviour()
    {
        foreach (var verticalScroll in _verticalScrolledPanels)
        {
            verticalScroll.DisableRect();
        }

        _needToSwitch = true;
    }

    private void ChangeScrollBehaviourBak()
    {
        _needToSwitch = false;
        foreach (var verticalScroll in _verticalScrolledPanels)
        {
            verticalScroll.EnableRect();
        }
    }

    private void EndScroll()
    {
        if(_switchByButtons)
            return;

        foreach (var verticalScroll in _verticalScrolledPanels)
        {
            if (Mathf.Abs(verticalScroll.PanelNormalizedPosition -_horizontalScrollRect.horizontalNormalizedPosition) < 0.4f && verticalScroll != _currentScrolledPanel)
            {
                _previousScrolledPanel = _currentScrolledPanel;
                _currentScrolledPanel = verticalScroll;
              break;
            }
        }
        
        DOTween.To(() => 
            _horizontalScrollRect.horizontalNormalizedPosition, x => _horizontalScrollRect.horizontalNormalizedPosition = x, _currentScrolledPanel.PanelNormalizedPosition, 0.75f);
        PanelSwitched();
    }

    private void  ScrollByButtons(ScrolledPanel panel)
    {
        if(!_horizontalScrollRect.enabled)
            return;
        
        _switchByButtons = true;
        _previousScrolledPanel = _currentScrolledPanel;
        _currentScrolledPanel = panel;

        DOTween.To(() => 
            _horizontalScrollRect.horizontalNormalizedPosition,
                x => _horizontalScrollRect.horizontalNormalizedPosition = x,
                _currentScrolledPanel.PanelNormalizedPosition, 0.75f)
            .OnComplete(()=>_switchByButtons = false);

        _previousScrolledPanel.ExitPanel();
        _currentScrolledPanel.EnterPanel();
    }

    private void PanelSwitched()
    {
        if(!_horizontalScrollRect.enabled || !_needToSwitch)
            return;
        
        _previousScrolledPanel.ExitPanel();
        _currentScrolledPanel.EnterPanel();
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
        _swipeInput.TouchEnded -= EndScroll;
        _swipeInput.HorizontalSwipe -= ChangeScrollBehaviour;
        _swipeInput.VerticalSwipe -= ChangeScrollBehaviourBak;
    }
}
