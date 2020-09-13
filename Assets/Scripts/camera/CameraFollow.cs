using UnityEngine;

namespace camera
{
    public class CameraFollow : MonoBehaviour
    {
        public float smoothness;
        public Transform targetObject;
        private Vector3 _initialOffset;
        private Vector3 _cameraPosition;

        public enum RelativePosition { InitialPosition, Position1, Position2 }
        public RelativePosition relativePosition;
        public Vector3 position1;
        public Vector3 position2;

        private void Start()
        {
            relativePosition = RelativePosition.InitialPosition;
            _initialOffset = transform.position - targetObject.position;
        }

        private void FixedUpdate()
        {
            _cameraPosition = targetObject.position + CameraOffset(relativePosition);
            transform.position = Vector3.Lerp(transform.position, _cameraPosition, smoothness*Time.fixedDeltaTime);
            transform.LookAt(targetObject);
        }

        Vector3 CameraOffset(RelativePosition ralativePos)
        {
            Vector3 currentOffset;

            switch (ralativePos)
            {
                case RelativePosition.Position1:
                    currentOffset = position1;
                    break;

                case RelativePosition.Position2:
                    currentOffset = position2;
                    break;

                default:
                    currentOffset = _initialOffset;
                    break;
            }
            return currentOffset;
        }
    }
}