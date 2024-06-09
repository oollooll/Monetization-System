using System;
using UnityEngine;

namespace Services
{
    public class SwipeInput : MonoBehaviour
    {
        [SerializeField] private SoundManager.SoundManager _soundManager;
        private Vector2 _fingerDownPosition;
        private Vector2 _fingerUpPosition;
        private float _minDistanceForSwipe = 20f;
        private bool _canBeSwiped;

        public event Action HorizontalSwipe;
        public event Action VerticalSwipe;
        public event Action TouchEnded;
        
        private void Update()
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    _soundManager.PlaySound(1);
                    _fingerDownPosition = touch.position;
                    _fingerUpPosition = touch.position;
                    _canBeSwiped = true;
                }

                if (touch.phase == TouchPhase.Moved)
                {
                    _fingerUpPosition = touch.position;
                    DetectSwipe();
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    _fingerUpPosition = touch.position;
                    DetectSwipe();
                    TouchEnded?.Invoke();
                }
            }
        }
        
        private void DetectSwipe()
        {
            if(!_canBeSwiped)
                return;
            
            if (Vector2.Distance(_fingerDownPosition, _fingerUpPosition) < _minDistanceForSwipe)
                return;

            Vector2 swipeDirection = _fingerUpPosition - _fingerDownPosition;
            
            if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
            {
                HorizontalSwipe?.Invoke();
            }
            else
            {
                VerticalSwipe?.Invoke();
            }

            _fingerDownPosition = _fingerUpPosition;
        }
    }
}