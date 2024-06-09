using System;
using Plane.Enemy;
using Services.ProjectUpdater;
using UnityEngine;

namespace Plane.Bullet
{
    public class Bullet:MonoBehaviour
    {
        private float _moveSpeed;
        private float _screenBoundZ = 10;

        public event Action ScoreUpdated;

        public void Initialize(float speed)
        {
            _moveSpeed = speed;
        }
        private void OnEnable()
        {
            ProjectUpdater.Instance.UpdateCalled += Move;
            ProjectUpdater.Instance.UpdateCalled += BoundsControl;
        }

        private void Move()
        {
            transform.Translate(Vector3.up * _moveSpeed * Time.deltaTime);
        }
        
        private void BoundsControl()
        {
            if (transform.position.z > _screenBoundZ)
            {
                gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out DarkStriker ds))
            {
                ds.DarkStrikerDefeat();
                
            }
            
            if (other.TryGetComponent(out EnemyPlane enemy))
            {
                enemy.OnHit();
                gameObject.SetActive(false);
                ScoreUpdated?.Invoke();
            }
        }
        
        private void OnDisable()
        {
            ProjectUpdater.Instance.UpdateCalled -= Move;
            ProjectUpdater.Instance.UpdateCalled -= BoundsControl;
        }
    }
}