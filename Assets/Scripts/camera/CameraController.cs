using input;
using UnityEngine;

namespace camera
{
    public class CameraController : MonoBehaviour
    {
        // public variables
        
        // private variables
        private Camera _cam;
        
        // events and delegates
        public delegate void CarouselCameraSetDelegate(Camera cam, Vector3 target, float zSize);
        public static event CarouselCameraSetDelegate CarouselCameraSet;
        

        private void Awake()
        {
            _cam = transform.GetComponent<Camera>();
        }

        private void OnEnable()
        {
            Carousel5.CarouselArranged += AlignWith;
        }
        
        private void OnDisable()
        {
            Carousel5.CarouselArranged -= AlignWith;
        }

        private void AlignWith(Vector3 carouselExtents, Vector3 lookPoint, Vector3 lookDirection)
        {
            const float margin = 0.65f;
            const float yAngle = 15.0f;
            var maxExtent = carouselExtents.magnitude;
            var minDistance = (maxExtent * margin) / Mathf.Sin(Mathf.Deg2Rad * _cam.fieldOfView / 2.0f);
            _cam.nearClipPlane = minDistance - maxExtent;
            // Debug.Log($"logs: {minDistance}, {maxExtent}");
            _cam.farClipPlane = 140;
            var transform1 = transform;
            transform1.position = lookPoint + lookPoint.normalized * minDistance + transform1.up * yAngle;
            transform1.rotation = Quaternion.LookRotation(-lookPoint);
            CarouselCameraSet?.Invoke(_cam, lookPoint, carouselExtents.z);
        }
        

    }
}