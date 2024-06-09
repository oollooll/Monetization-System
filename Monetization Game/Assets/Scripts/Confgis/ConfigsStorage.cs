using System.Collections.Generic;
using UnityEngine;

namespace Confgis
{
    [CreateAssetMenu(fileName = "ConfigsStorage", menuName = "MonetizationConfigs", order = 1)]
    public class ConfigsStorage : ScriptableObject
    {
        [field:SerializeField] public List<Config> MonetizationCofigs { get; private set; }
    }
}
