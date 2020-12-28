using System;
using UnityEngine;

namespace camera
{
    public class CameraController : MonoBehaviour
    {
        public float cameraDistance = ScreenConstants.DefaultDepth;
        private Transform _target;
        private Camera cam;

        public void SetTarget(Transform target)
        {
            _target = target;
            // Debug.DrawRay(transform.position, -(target.position - Vector3.zero), Color.blue, 20f);
        }

        private void Start()
        {
            cam = transform.GetComponent<Camera>();
        }

        private void Update()
        {
            if (_target == null) return;
            var newPos = (_target.position - Vector3.zero) * cameraDistance;
            
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(-newPos.normalized),
                Time.fixedDeltaTime * 2.0f);
            
            transform.position = Vector3.Lerp(transform.position, newPos, Time.fixedDeltaTime * 2.0f);

            var bounds = _target.GetComponent<MeshFilter>().mesh.bounds;

            var p1 = cam.WorldToViewportPoint(bounds.min);
            var p2 = cam.WorldToViewportPoint(bounds.max);
            
            Debug.Log($"p1: {p1}, p2: {p2}");
            
        }
    }
}