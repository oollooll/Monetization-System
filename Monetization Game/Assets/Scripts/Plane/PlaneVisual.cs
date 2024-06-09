using UnityEngine;

namespace Plane
{
    public class PlaneVisual:MonoBehaviour
    {
        [field:SerializeField] public Material MaterialPlane { get; private set; }
        [field:SerializeField] public Transform TrasformWings { get; private set; }
        [field:SerializeField] public Color Color { get; private set; }
        
    }
}