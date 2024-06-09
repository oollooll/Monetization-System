using System;
using UnityEngine;

namespace Plane.HangarData
{
    [Serializable]
    public class ItemHangarData
    {
        [field: SerializeField] public int Index { get; private set; }
        [field: SerializeField] public int Price { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
    }
}