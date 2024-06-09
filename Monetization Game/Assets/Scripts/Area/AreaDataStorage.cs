using System.Collections.Generic;
using UnityEngine;

namespace Area
{
    [CreateAssetMenu(fileName = "AreaDataStorage", menuName = "AreaDataStorage", order = 1)]
    public class AreaDataStorage:ScriptableObject
    {
        [field: SerializeField] public List<AreaData> AreasList { get; private set; }
    }
}