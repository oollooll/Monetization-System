using Confgis;
using UnityEngine;

namespace Core
{
    public class MonetizationManager : MonoBehaviour
    {
        [SerializeField] private ConfigsStorage _configsStorage;
        
        [SerializeField] private bool AddAdvertisment;
        [ConditionalField("AddAdvertisment")] 
        public ADSManager AdsManager;
        
        [SerializeField] private bool AddPushNotification;
        [ConditionalField("AddPushNotification")] 
        public NotificationManager NotificationManager;
        
        [SerializeField] private bool AddInAppPurchasing;
        [ConditionalField("AddInAppPurchasing")] 
        public IAPController IAPController;
        
        [SerializeField] private bool AddAnalytics;
        [ConditionalField("AddAnalytics")] 
        public AnalyticManager AnalyticManager;
    }
}



