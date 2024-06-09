using System;
using UnityEngine;
using UnityEngine.UI;

namespace Plane.Enemy
{
    public class EnemyCrash : MonoBehaviour
    {
        public event Action ScoreUpdated;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out EnemyPlane enemy))
            {
                enemy.OnHit();
                ScoreUpdated?.Invoke();
            }
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out EnemyPlane enemy))
            {
                enemy.OnHit();
                ScoreUpdated?.Invoke();
            }
        }
    }
}