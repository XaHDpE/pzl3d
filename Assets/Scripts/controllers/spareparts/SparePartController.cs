using System.Collections.Generic;
using events;
using Lean.Touch;
using UnityEngine;

namespace models.sp
{
    [RequireComponent(typeof(LeanDragTranslate))]
    [RequireComponent(typeof(LeanPitchYaw))]
    [RequireComponent(typeof(LeanFingerSwipe))]
    public class SparePartController : SparePartBase
    {
        [SerializeField] private CustomEventsManager eventManager;
        // TODO move this declaration somewhere

        public bool rotationEnabled;
        public bool movementEnabled;

        // scripts
        private LeanDragTranslate _ldt;
        private LeanPitchYaw _lpy;
        private LeanFingerSwipe _lfs;
    
        // components

        public bool verticesDone;
    
        private List<EdgeVector> _edges;

        protected override void Awake()
        {
            base.Awake();

            PropagateSelectionToParent(true);
            
            _ldt = GetComponent<LeanDragTranslate>();
            _lpy = GetComponent<LeanPitchYaw>();
            _lfs = GetComponent<LeanFingerSwipe>();
            
            _edges = new List<EdgeVector>();
            
        }
        
        private void EnableMovement()
        {
            // Lean drag translate
            _ldt.enabled = true;
            _ldt.Use = new LeanFingerFilter {RequiredSelectable = GetLeanSelectable()};
            print($"movement enabled for {name}");
        }
        private void DisableMovement()
        {
            _ldt.enabled = false;
            print($"movement disabled for {name}");
        }

        private void EnableRotation()
        {
            _lfs.enabled = true;
            _lpy.enabled = true;
            // Lean finger swipe
            _lfs.RequiredSelectable = GetLeanSelectable();
            _lfs.Multiplier = 45f;
            _lfs.OnDelta.AddListener(_lpy.Rotate);
            print($"rotation enabled for {name}");
        }
    
        private void DisableRotation()
        {
            _lfs.enabled = false;
            _lpy.enabled = false;
            print($"rotation disabled for {name}");
        }

        protected override void OnSelectHandler(LeanFinger finger)
        {
            base.OnSelectHandler(finger);
            // CheckMesh();
            // EnableMovement();
            // MoveToCameraPivot();
            // CenterRelativeToScreen();
            // MoveToDefaultLayer();
            InvokeSparePartSelectedEvent(transform);
            // EnableRotation();
            // 
        }

        private void MoveToCameraPivot()
        {
            transform.SetParent(GameObject.FindWithTag("CameraPivot").transform);
        }

        private void CenterRelativeToScreen()
        {
            // get camera from parent
            var newPos = GetMainCamera().transform.localPosition;
            newPos.z += 5f;
            transform.localPosition = newPos;

        }

        private void MoveToDefaultLayer()
        {
            transform.gameObject.layer = LayerMask.NameToLayer("Default");
        }
    
        public override Transform GetSparePart()
        {
            return transform;
        }

        private void DrawPlane(Vector3 p1, Vector3 p2)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Plane);
            var centerPos = p2 - p1;
            go.transform.position = p1 + (p2 - p1) / 2.0f;
            var localScale = transform.localScale;
            localScale.y = 0.02f;
            go.transform.localScale = localScale;
            go.transform.up = centerPos;
        }

        /*
        private void UnderRayCast(Transform t, int subMeshIndex)
        {
            if (!t.Equals(transform)) return;
            if (_edges.Count > 0) return;
            _edges = CalculateSubMeshEdges(subMeshIndex);
        
            foreach (var edgeVec in _edges)
            {
                CollidersHelper.GenerateBoundBoxCollider(transform, edgeVec);
            }
        }
        */

        void OnGUI() {
            /*if (_edges.Count == 0) return;
            if (verticesDone) return;
            for (var i = 0; i < GetMesh().vertexCount; i++) {
                Handles.matrix = GetComponent<MeshFilter>().transform.localToWorldMatrix;
                Handles.color = Color.yellow;
                Handles.DrawLine(
                    GetMesh().vertices[i],
                    GetMesh().vertices[i] + GetMesh().normals[i]);
            }

            verticesDone = true;*/
        }

        protected override void OnDeSelectHandler()
        {
            base.OnDeSelectHandler();
            DisableMovement();
            DisableRotation();
        }

    }
}