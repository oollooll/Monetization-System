using System.Collections.Generic;
using UnityEngine;

namespace Plane.HangarData
{
    [CreateAssetMenu(fileName = "HangarDataStorage", menuName = "HangarDataStorage", order = 1)]
    public class HangarDataStorage : ScriptableObject
    {
        [field: SerializeField] public List<ItemHangarData> ItemHangarDatas { get; private set; }
    }
}