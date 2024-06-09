using System;
using System.Collections;
using System.Collections.Generic;
using Plane;
using Plane.HangarData;
using Services.ScoreDir;
using UnityEngine;

public class HangarShop : MonoBehaviour,IScoreUpdater
{
    [SerializeField] private HangarDataStorage _hangarDataStorage;
    [SerializeField] private List<ItemHangar> _itemsHangar;
    [SerializeField] private Sprite _select;
    [SerializeField] private Sprite _selected;
    private int _currentPlane;
    
    public event IScoreUpdater.ScoreUpdated OnScoreUpdate;

    private void Start()
    {
        _currentPlane = PlayerPrefs.GetInt("CurrentPlane");
        foreach (var itemHangar in _itemsHangar)
        {
            Sprite sprite; 
            var data = _hangarDataStorage.ItemHangarDatas[_itemsHangar.IndexOf(itemHangar)];
            
            if (_itemsHangar.IndexOf(itemHangar) == _currentPlane)
                sprite = _selected;
            else
                sprite = _select;

            itemHangar.Initialize(data.Index,data.Price,data.Name,sprite);
            itemHangar.UnlockPlane += BuyPlane;
            itemHangar.SelectPlane += SelectPlane;
        }
    }

    private void BuyPlane(ItemHangar itemHangar)
    {
        var currentScore =  PlayerPrefs.GetInt("Coins");
        if (currentScore < itemHangar.Price)
        {
            return;
        }
            
        PlayerPrefs.SetInt(itemHangar.Name,1);
        PlayerPrefs.Save();

        OnScoreUpdate?.Invoke(-itemHangar.Price);
        itemHangar.InitializeButton(_select);
    }

    private void SelectPlane(ItemHangar itemHangar)
    {
        _itemsHangar[_currentPlane].ChangeButtonSprite(_select);
        _currentPlane = itemHangar.Index;
        itemHangar.ChangeButtonSprite(_selected);
        PlayerPrefs.SetInt("CurrentPlane",_currentPlane);
        PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        foreach (var itemHangar in _itemsHangar)
        {
            itemHangar.UnlockPlane -= BuyPlane;
            itemHangar.SelectPlane -= SelectPlane;
        }
    }
}
