using UnityEngine;
using SystemInfo = UnityEngine.Device.SystemInfo;

namespace Services.VibrationManagement
{
    public class VibrationManager : MonoBehaviour
    {
        public void TriggerVibration()
        {
            if (SystemInfo.supportsVibration)
            {
                if(!CheckVibroStatus())
                    return;
                
                Handheld.Vibrate();;
            }
        }
        
        private bool CheckVibroStatus()
        {
            var soundOn = PlayerPrefs.GetInt("VibroOn");

            if (soundOn == 0)
                return false;
            else
                return true;
        }
    }
}