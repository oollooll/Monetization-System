using System;
using DG.Tweening;
using Services.ProjectUpdater;
using Services.SoundManager;
using Services.VibrationManagement;
using UnityEngine;

namespace Plane.Enemy
{
    public class EnemyPlane:AbstractPlane
    {
        [SerializeField] private ParticleSystem _boomFx;
        [SerializeField] private GameObject _body;
        [SerializeField] private GameObject _propeller;
        [SerializeField] private Collider _collider;
        
        private SoundManager _soundManager;
        private VibrationManager _vibrationManager;
        
        private int _screenBoundZ = 9;
        private ProjectUpdater _projectUpdater;

        public void Initialize(float speed, SoundManager soundManager,VibrationManager vibrationManager)
        {
            base.Initialize(speed);
            _soundManager = soundManager;
            _vibrationManager = vibrationManager;
        }

        private void OnEnable()
        {
            _body.SetActive(true);
            _propeller.SetActive(true);
            _collider.enabled = true;
            
            ProjectUpdater.Instance.UpdateCalled += Move;
            ProjectUpdater.Instance.UpdateCalled += BoundsControl;
        }

        public override void Move()
        {
            transform.Translate(Vector3.forward * _moveSpeed * Time.deltaTime);
        }

        public void OnHit()
        {
            _body.SetActive(false);
            _propeller.SetActive(false);
            _collider.enabled = false;
            
            _boomFx.Play();
            _soundManager.PlaySound(1);
            _vibrationManager.TriggerVibration();
        }
        
        private void BoundsControl()
        {
            if (transform.position.z < -_screenBoundZ)
            {
                gameObject.SetActive(false);
            }
        }

        private void OnDisable()
        {
            ProjectUpdater.Instance.UpdateCalled -= Move;
            ProjectUpdater.Instance.UpdateCalled -= BoundsControl;
        }
    }
}