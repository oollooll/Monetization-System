using System;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenuControl
{
    public class Settings:MonoBehaviour
    {
        [SerializeField] private Slider _sensitivitySlider;

        private void Start()
        {
            _sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
        }

        private void OnEnable()
        {
            _sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity");
        }

        private void SetSensitivity(float value)
        {
            PlayerPrefs.SetFloat("Sensitivity",value);
            PlayerPrefs.Save();
        }
    }
}