using events;
using Lean.Touch;
using UnityEngine;

namespace camera
{
    [RequireComponent(typeof(LeanPinchCamera))]
    [RequireComponent(typeof(LeanMultiPinch))]
    public class CameraController : MonoBehaviour
    {
        struct RectData
        {
            public Rect init;
            public Rect curr;
            public Rect max;
        };

        public float lerpSpeed = 0.5f;

        private Camera _cam;
        public CustomEventsManager ec;
        private Rect _rect;
        private LeanPinchCamera _lpc;
        private LeanMultiPinch _lmp;

        private RectData _rectData;

        private void Awake()
        {
            _cam = GetComponent<Camera>();
            _lpc = GetComponent<LeanPinchCamera>();
            _lmp = GetComponent<LeanMultiPinch>();

            _rectData = new RectData()
                {init = _cam.rect, curr = _cam.rect, max = new Rect(0, 0, 1, 1)};
        }

        private void OnEnable()
        {
            // SparePartBase.SelectedNotification += ResizeViewport;
            // _lmp.OnPinch.AddListener(ChangeViewport);
        }

        private void OnDisable()
        {
            // SparePartBase.SelectedNotification -= ResizeViewport;
            // _lmp.OnPinch.RemoveAllListeners();
        }

        private void Start()
        {
            _rect = _cam.rect;
        }

        private void ChangeViewport(float factor)
        {
            print($"ChangeViewport, factor: {factor}, CoordinateType: {_lmp.Coordinate}");
            _cam.depth = 999;

            /*_rectData.curr = LerpRect(
                _rectData.curr, 
                _rectData.max, 
                factor * 0.05f);*/

            _cam.rect = _rectData.curr;
        }

        private Rect LerpRect1(Rect nRect1, Rect nRect2, float nNum)
        {
            var vRect = nRect1;
            vRect.x += (nRect2.x - nRect1.x) * nNum;
            vRect.y += (nRect2.y - nRect1.y) * nNum;
            vRect.width += (nRect2.width - nRect1.width) * nNum;
            vRect.height += (nRect2.height - nRect1.height) * nNum;
            return (vRect);
        }

        private Rect LerpRect(Rect nRect1, Rect nRect2, float nNum)
        {
            var vRect = nRect1;
            vRect.x += (nRect2.x - nRect1.x) * nNum;
            vRect.y += (nRect2.y - nRect1.y) * nNum;
            vRect.width += (nRect2.width - nRect1.width) * nNum;
            vRect.height += (nRect2.height - nRect1.height) * nNum;
            return (vRect);
        }

        private void FixedUpdate()
        {
            var ray = _cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));

            if (!Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, 1 << 8)) return;
            var smUnderRay = hitInfo.transform.GetComponent<MeshFilter>().sharedMesh;
            // var subMeshIndex = MaterialHelper.GetSubMeshIndex(smUnderRay, hitInfo.triangleIndex);
            // ec.SparePartUnderRayCast(hitInfo.transform, subMeshIndex);
        }
    }
}