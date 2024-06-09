using System.Collections.Generic;
using Services.ScoreDir;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Area
{
    public class AreaController : MonoBehaviour , IScoreUpdater
    {
        [SerializeField] private List<Area> _areas;
        [SerializeField] private AreaDataStorage _areaDataStorage;
        [SerializeField] private GameObject _unlockPanel;
        [SerializeField] private GameObject _notEnough;
        [SerializeField] private TextMeshProUGUI _price;
        [SerializeField] private TextMeshProUGUI _areaName;
        [SerializeField] private Button _buy;

        private int _unlockedIndex;
        public event IScoreUpdater.ScoreUpdated OnScoreUpdate;
        
        private void Start()
        {
            _unlockedIndex = PlayerPrefs.GetInt("UnlockArea",0);
            for (int i = 0; i <= _unlockedIndex; i++)
            {
                var area = _areaDataStorage.AreasList[i];
                _areas[i].Initialize(area.AreaNormal,i,true,area.AreaPrice,area.AreaName);
            }

            for (int i = _unlockedIndex+1; i < _areas.Count; i++)
            {
                var area = _areaDataStorage.AreasList[i];
                _areas[i].Initialize(area.AreaLock,i,false,area.AreaPrice,area.AreaName);
                _areas[i].LockedArea += ShowUnlockPanel;
            }
        }

        private void ShowUnlockPanel(Area area)
        {
            _unlockPanel.SetActive(true);
            _price.text = $"{area.Price}";
            _areaName.text = area.AreaName;
            
            var currentScore =  PlayerPrefs.GetInt("Coins");
            if (currentScore < area.Price)
            {
                _notEnough.SetActive(true);
                _buy.interactable = false;
            }
            else
            {
                _buy.interactable = true;
                _buy.onClick.AddListener(() => BuyArea(area));
            }
        }

        private void BuyArea(Area area)
        {
            var currentScore =  PlayerPrefs.GetInt("Coins");
            if (currentScore < area.Price)
            {
               return;
            }
            
            PlayerPrefs.SetInt("UnlockArea",area.IndexArea);
            PlayerPrefs.Save();
            
            OnScoreUpdate?.Invoke(-area.Price);
            
            for (int i = 0; i <= area.IndexArea; i++)
            {
                var areat = _areaDataStorage.AreasList[i];
                _areas[i].Initialize(areat.AreaNormal,i,true,areat.AreaPrice,areat.AreaName);
            }
            _buy.interactable = false;
        }
    }
}
