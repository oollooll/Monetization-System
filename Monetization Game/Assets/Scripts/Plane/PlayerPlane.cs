using System;
using System.Collections.Generic;
using DG.Tweening;
using Plane.Enemy;
using Services.ProjectUpdater;
using TMPro;
using UnityEngine;

namespace Plane
{
    public class PlayerPlane:AbstractPlane
    {
        [SerializeField] private List<PlaneVisual> _planes;
        [SerializeField] private ParticleSystem _boomFx;
        [SerializeField] private TextMeshProUGUI _hpText;
        
        private Transform _wingsTransform;
        private bool _isRotatedX;
        private bool _isRotatedY;
        private float _sensitivity;
        private float _boundY = 2.5f;
        private float _boundX = 1.5f;
        private int _hp;
        private Color _startColor;
        private Material _planeMaterial;

        public event Action GameLoss;
        
        public override void Initialize(float speed)
        {
            base.Initialize(speed);

            var currentPlane = PlayerPrefs.GetInt("CurrentPlane");
            _sensitivity = PlayerPrefs.GetFloat("Sensitivity");
            _wingsTransform = _planes[currentPlane].TrasformWings;
            _planeMaterial = _planes[currentPlane].MaterialPlane;
            _planes[currentPlane].gameObject.SetActive(true);
            _hp = 3;
            _hpText.text = $"x{_hp}";
            _startColor = _planes[currentPlane].Color;
            _planeMaterial.color = _startColor;
            ProjectUpdater.Instance.UpdateCalled += Move;
            ProjectUpdater.Instance.UpdateCalled += BoundsControl;
        }
        
        
        public override void Move()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                
                if (touch.phase == TouchPhase.Moved)
                {
                    Vector2 touchPosition = touch.deltaPosition;
                   
                    transform.Translate(new Vector3(0f, touchPosition.y, touchPosition.x) * _sensitivity * Time.deltaTime);
                    WingsRotation(touchPosition);
                }
            }
            else
            {
                if(!_isRotatedY && !_isRotatedX)
                    return;
                
                _wingsTransform.DORotate(new Vector3(_wingsTransform.rotation.x, 90f, 0f), 0.5f);
                _isRotatedY = false;
                _isRotatedX = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out EnemyPlane enemy))
            {
                HitAnim();
                UpdateHp();
                
                if (_hp == 0)
                {
                    enemy.OnHit();
                    _boomFx.Play();
                    gameObject.SetActive(false);
                    GameLoss?.Invoke();
                }
            }
        }

        private void UpdateHp()
        {
            _hp--;
            _hpText.text = $"x{_hp}";
        }

        private void HitAnim()
        {
            var s = DOTween.Sequence();
            for (int i = 0; i < 3; i++)
            {
                s.Append(_planeMaterial.DOColor(Color.red, 0.3f));
                s.Append(_planeMaterial.DOColor(_startColor, 0.3f));
            }
            _planeMaterial.color = _startColor;
        }

        private void WingsRotation(Vector2 input)
        {
            var inputX = Mathf.Clamp01(Mathf.Abs(input.x));
            var inputY = Mathf.Clamp01(Mathf.Abs(input.y));
            
            if(Mathf.Abs(inputY) == 0 && Mathf.Abs(inputX) == 0)
            {
                _wingsTransform.DORotate(new Vector3(_wingsTransform.rotation.x, 90f, 0f), 0.5f);
                _isRotatedY = false;
                _isRotatedX = false;
            }
            else
            {
                float tiltX = Mathf.Sign(input.x) * 30;
                float tiltY = Mathf.Sign(input.y) * 30;
                
                _wingsTransform.DORotate(new Vector3(tiltX, 90f, -tiltY), 0.5f); 
                
                if(tiltX != 0)
                    _isRotatedX = true;
                if(tiltY != 0)
                    _isRotatedY = true;
            }
        }

        private void BoundsControl()
        {
            if (transform.position.x > _boundX)
            {
                transform.position = new Vector3(_boundX, transform.position.y, transform.position.z);
            }
            else if (transform.position.x < -_boundX)
            {
                transform.position = new Vector3(-_boundX, transform.position.y, transform.position.z);
            }
            
            if (transform.position.y > _boundY)
            {
                transform.position = new Vector3(transform.position.x, _boundY, transform.position.z);
            }
            else if (transform.position.y < -_boundY)
            {
                transform.position = new Vector3(transform.position.x, -_boundY, transform.position.z);
            }
        }

        private void OnDestroy()
        {
            ProjectUpdater.Instance.UpdateCalled -= Move;
            ProjectUpdater.Instance.UpdateCalled -= BoundsControl;
        }
    }
}