using UnityEngine;

namespace Services
{
    public class ResizeBackground : MonoBehaviour
    {
       
        private Vector2 _screenSize;

        private void Start()
        {
            _screenSize = new Vector2(Screen.width, Screen.height);

            ScaleWithScreenSize();
        }

        private void ScaleWithScreenSize()
        {
            float scaleX = _screenSize.x / _screenSize.y;
            
            transform.localScale= new Vector3(2*scaleX, transform.localScale.y);
        }
    }
}