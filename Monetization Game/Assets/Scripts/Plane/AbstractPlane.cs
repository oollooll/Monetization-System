using UnityEngine;

namespace Plane
{
    public abstract class AbstractPlane : MonoBehaviour
    {
        protected float _moveSpeed;

        public virtual void Initialize( float speed)
        {
            _moveSpeed = speed;
        }

        public abstract void Move();
    }
}