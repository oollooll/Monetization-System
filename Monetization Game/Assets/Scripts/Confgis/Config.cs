using System;
using Enums;
using UnityEngine;

namespace Confgis
{
    [Serializable]
    public class Config
    {
        [field:SerializeField] public ConfigType ConfigType { get; private set; }
        [field:SerializeField] public string ApiKey { get; private set; }
    }
}