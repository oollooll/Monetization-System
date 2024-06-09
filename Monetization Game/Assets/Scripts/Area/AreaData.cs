using System;
using UnityEngine;

namespace Area
{
    [Serializable]
    public class AreaData
    {
        [field: SerializeField] public string AreaName { get; private set; }
        [field: SerializeField] public Sprite AreaSprite { get; private set; }
        [field: SerializeField] public Sprite AreaNormal { get; private set; }
        [field: SerializeField] public Sprite AreaLock{ get; private set; }
        [field: SerializeField] public int AreaIndex { get; private set; }
        [field: SerializeField] public int ScoreToPass { get; private set; }
        [field: SerializeField] public int LevelModif { get; private set; }
        [field: SerializeField] public int AreaPrice { get; private set; }
    }
}