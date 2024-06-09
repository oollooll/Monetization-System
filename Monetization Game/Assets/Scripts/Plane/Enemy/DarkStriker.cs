using UnityEngine;

namespace Plane.Enemy
{
    public class DarkStriker:EnemyPlane
    {
        public void DarkStrikerDefeat()
        {
            PlayerPrefs.SetInt("DarkStriker",1);
            PlayerPrefs.Save();
        }
    }
}