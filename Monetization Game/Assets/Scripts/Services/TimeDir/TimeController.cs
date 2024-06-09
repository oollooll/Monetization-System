using System;
using UnityEngine;

namespace Services.TimeDir
{
    public class TimeController
    {
        public void SaveUnlockTime()
        {
            DateTime currentTime = DateTime.Now;
            PlayerPrefs.SetString("SavedTime", currentTime.ToString());
            PlayerPrefs.Save();
        }

        public void LoadAndDisplayTime(ref int hours,ref int minutes , ref int seconds)
        {
            if(PlayerPrefs.HasKey("SavedTime"))
            {
                string savedTimeAsString = PlayerPrefs.GetString("SavedTime");
                long binaryTime = Convert.ToInt64(savedTimeAsString);
                
                DateTime savedTime = DateTime.FromBinary(binaryTime);
                hours = savedTime.Hour - DateTime.Now.Hour;
                minutes =  savedTime.Minute - DateTime.Now.Minute;
                seconds = savedTime.Second - DateTime.Now.Second;
            }
        }
    }
}