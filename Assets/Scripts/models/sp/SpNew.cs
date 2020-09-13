using System;
using gizmo;
using Lean.Touch;
using settings;
using UnityEditor.Rendering;
using UnityEngine;

namespace models.sp
{
    public struct InitProperties {
        public InitProperties(Vector3 relativePosition, Quaternion relativeRotation)
        {
            RelativePosition = relativePosition;
            RelativeRotation = relativeRotation;
        }
        public Vector3 RelativePosition { get; }

        public Quaternion RelativeRotation { get; }

        public override string ToString()
        {
            return $"InitProperties[RelativePosition: {RelativePosition}, RelativeRotation: {RelativeRotation}]";
        }
    }
    
    [RequireComponent(typeof(LeanMultiUpdate))]
    [RequireComponent(typeof(LeanThresholdDelta))]
    [RequireComponent(typeof(LeanManualRotate))]
    [RequireComponent(typeof(LeanMultiUpdate))]
    [RequireComponent(typeof(ShowMeshBounds))]
    [RequireComponent(typeof(LeanSelectable))]
    
    public class SpNew : SparePartBase
    {
        [SerializeField] public InitProperties initProperties;
        [SerializeField] public Camera renderingCamera;

        private LeanThresholdDelta _ltd;
        private LeanManualRotate _lmr;
        private LeanMultiUpdate _lmu;
        private LeanSelectable _ls;
        private GameManager _gameManager;
        
        public override Transform GetSparePart()
        {
            return transform;
        }

        protected override void Awake()
        {
            base.Awake();

            _ltd = GetComponent<LeanThresholdDelta>();
            _lmr = GetComponent<LeanManualRotate>();
            _lmu = GetComponent<LeanMultiUpdate>();
            _ls = GetComponent<LeanSelectable>();

            _gameManager = FindObjectOfType<GameManager>();

            // scripts configuration            
            _ltd.Threshold = 45f;
            _ltd.Step = true;
            
            _lmu.Coordinate = LeanMultiUpdate.CoordinateType.ScreenPixels;
            _lmu.Multiplier = 1;
            
            _lmu.Use = 
                new LeanFingerFilter(LeanFingerFilter.FilterType.AllFingers, true, 0, 0, _ls);
            _lmu.ScreenDepth = new LeanScreenDepth() { Camera = renderingCamera };
            
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            
            _ltd.OnDeltaXY.AddListener(_lmr.RotateAB);
            _lmu.OnDelta.AddListener(_ltd.AddXY);
            
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            _ltd.OnDeltaXY.RemoveAllListeners();
            _lmu.OnDelta.RemoveAllListeners();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, new Vector3(1, 1, 1));
        }

        protected override void OnSelectHandler(LeanFinger finger)
        {
            base.OnSelectHandler(finger);
            InvokeSparePartSelectedEvent(transform);
        }

        protected override void OnDeSelectHandler()
        {
            base.OnDeSelectHandler();
            InvokeSparePartDeselectedEvent();
        }
    }

}