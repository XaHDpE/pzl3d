using System;
using System.Collections.Generic;
using Lean.Touch;
using models.mesh;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace models.sparepart
{
    [RequireComponent(typeof(LeanMultiUpdate))]
    [RequireComponent(typeof(LeanThresholdDelta))]
    [RequireComponent(typeof(LeanManualRotate))]
    [RequireComponent(typeof(LeanMultiUpdate))]
    [RequireComponent(typeof(LeanFingerTap))]
    public class SpNb0511 : SparePartBase
    {
        [SerializeField] private InitProperties initProps;
        [SerializeField] public Tris3 selectedTris;

        // lean components
        private LeanThresholdDelta _ltd;
        private LeanManualRotate _lmr;
        private LeanMultiUpdate _lmu;
        private LeanFingerTap _lft;
        private LeanDragTranslateAlong _ldta;

        
        public GameObject leanPlanePrefab;
        private PlaneManager _planeManager;

        private int _selectedTrisIndex;
        
        private Button _normalsButton;

        private bool _startAlignRotation;
        private bool _startMove;

        private Vector3 _targetRotation;
        private Vector3 _currentRotation;
        private Vector3 _targetPosition;

        private float _curTime;

        // private GameManager _gameManager;
        
        // events
        public static event Action<SpNb0511> TrisSelectedEvent = (sp) => { };
        
        public override Transform GetSparePart()
        {
            return transform;
        }

        private void CacheLeanComponents()
        {
            _ltd = GetComponent<LeanThresholdDelta>();
            _lmr = GetComponent<LeanManualRotate>();
            _lmu = GetComponent<LeanMultiUpdate>();
            _lft = GetComponent<LeanFingerTap>();
            _ldta = GetComponent<LeanDragTranslateAlong>();
        }

        private void ConfigLeanComponents()
        {
            _ltd.Threshold = 45f;
            _ltd.Step = true;
            
            _lmu.Coordinate = LeanMultiUpdate.CoordinateType.ScreenPixels;
            _lmu.Multiplier = 1;
            
            _lmu.Use = new LeanFingerFilter(
                LeanFingerFilter.FilterType.AllFingers, 
                true, 
                0, 0, 
                GetLeanSelectable()
                );
            
            _lmu.ScreenDepth = new LeanScreenDepth() { Camera = CurrentCamera };

            _lft.RequiredTapCount = 2;

        }

        protected override void Awake()
        {
            base.Awake();

            CacheLeanComponents();
            ConfigLeanComponents();

            // _omt = GetComponent<OneMoreTimeScript>();
            _normalsButton = GameObject.FindWithTag("NormalsButton").GetComponent<Button>();
            _planeManager = Instantiate(leanPlanePrefab, Vector3.zero, Quaternion.identity).GetComponent<PlaneManager>();
            
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _ltd.OnDeltaXY.AddListener(_lmr.RotateAB);
            _lmu.OnDelta.AddListener(_ltd.AddXY);
            _lft.OnFinger.AddListener(InterceptDoubleClickEvent);
            _normalsButton.onClick.AddListener(ShowNormalsAndTris);
            
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            _ltd.OnDeltaXY.RemoveAllListeners();
            _lmu.OnDelta.RemoveAllListeners();
            _lft.OnFinger.RemoveAllListeners();
            _normalsButton.onClick.RemoveAllListeners();
            
        }

        /*private void OnDrawGizmosSelected()
        {
            // Draws a 5 unit long red line in front of the object
            Gizmos.color = Color.red;
            var direction = transform.TransformDirection(Vector3.forward) * 5;
            Gizmos.DrawRay(transform.position, direction);
            if (selectedTris != null)
            {
                Gizmos.color = Color.green;
                var direction1 = selectedTris.NormalGlobal * 5;
                Gizmos.DrawRay(selectedTris.MiddlePointGlobal, direction1);
            }
        }*/

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

        private void InterceptDoubleClickEvent(LeanFinger finger)
        {
            var ray = GetMainCamera().ScreenPointToRay(finger.ScreenPosition);
            if (!Physics.Raycast(ray, out var hit)) return;
            _selectedTrisIndex = hit.triangleIndex;
            selectedTris = new Tris3(transform, GetMesh().triangles, hit.triangleIndex);
            var targetTransform = hit.transform.GetComponent<SpNb0511>();
            TrisSelectedEvent?.Invoke(this);
        }

        public void OverridenRotation()
        {
            
            _ltd.OnDeltaXY.RemoveAllListeners();
            _ltd.OnDeltaY.AddListener(_lmr.RotateA);
            selectedTris.Debug("normal", Color.cyan, 10);
            _lmr.AxisA = selectedTris.NormalGlobal;
        }

        public void MoveRotateTo(Vector3 rotation, Vector3 position)
        {
            // remember the last position
            initProps = new InitProperties(transform.position, transform.rotation);

            _targetRotation = rotation;
            _targetPosition = position;
            
            _startAlignRotation = true;
        }
        
        public void StepBack()
        {
            _targetRotation = initProps.Rot.eulerAngles;
            _targetPosition = initProps.Pos;
            _startAlignRotation = true;
        }
        
        /*private void Update()
        {
            const float speed = 1.0f;
            
            if (_startAlignRotation)
            {
                var singleStep = speed * Time.deltaTime;
                var newDirection = Vector3.RotateTowards(
                    _currentRotation, 
                    _targetRotation, 
                    singleStep, 
                    0.0f);
                
                transform.rotation = Quaternion.LookRotation(newDirection);

                if (Vector3.Distance(transform.rotation.eulerAngles, _targetPosition) < 0.0009)
                {
                    _startAlignRotation = false;
                    curTime = 0f;
                    selectedTris.Debug(selectedTris.NormalGlobal.ToString(), Color.red, 30);
                }                
            }
        }*/
        
        
        private void Update1()
        {
            var speed = 5.0f;

            if (_startAlignRotation)
            {
                var intermediateTgt = _targetPosition - ( selectedTris.MiddlePointGlobal - transform.position);
                if (Vector3.Distance(transform.eulerAngles, _targetRotation) > 0.01f)
                {
                    transform.eulerAngles = Vector3.Lerp(
                        transform.rotation.eulerAngles, 
                        _targetRotation, 
                        speed * Time.deltaTime);
                }

                if (Vector3.Distance(transform.position, intermediateTgt) > 0.01f)
                {
                    transform.position = Vector3.Lerp(
                        transform.position,
                        intermediateTgt, 
                        speed * Time.deltaTime);
                }
                
                if (
                    (Vector3.Distance(transform.eulerAngles, _targetRotation) < 0.01f) &&
                    (Vector3.Distance(transform.position, _targetPosition) < 0.01f)
                    )
                {
                    transform.eulerAngles = _targetRotation;
                    transform.position = _targetPosition;
                    _startAlignRotation = false;
                    selectedTris.Debug(selectedTris.NormalGlobal.ToString(), Color.green, 30);
                } 
                
            }
            
        }
        
        
        private void Update()
        {
            var sec = 2.0f;
            if (_startAlignRotation)
            {
                _curTime += Time.deltaTime;
                var tm = Mathf.Clamp(_curTime / sec, 0f, 1f);

                transform.eulerAngles = Vector3.Lerp(initProps.Rot.eulerAngles, _targetRotation, tm);
                
                transform.position = Vector3.Lerp(initProps.Pos, _targetPosition - (selectedTris.MiddlePointGlobal - transform.position), tm);

                if (Math.Abs(tm - 1f) < 0.00001)
                {
                    _startAlignRotation = false;
                    _curTime = 0f;
                    
                    // selectedTris.Debug(selectedTris.NormalGlobal.ToString(), Color.magenta, 30);
                    _planeManager.MovePlaneAlong(transform.position, selectedTris.NormalGlobal);
                    var ldta = gameObject.AddComponent<LeanDragTranslateAlong>();
                    ldta.ScreenDepth = new LeanScreenDepth()
                    {
                        Conversion = LeanScreenDepth.ConversionType.PlaneIntercept,
                        Object = _planeManager.GetComponent<LeanPlane>()
                    };
                }                
            }
        }
        
        

        private bool _verticesDone;
        private ICollection<EdgeVector> _edges = new List<EdgeVector>();
        
        /*void OnGUI() {
            if (_edges.Count == 0) return;
            if (_verticesDone) return;
            for (var i = 0; i < GetMesh().vertexCount; i++) {
                Handles.matrix = GetComponent<MeshFilter>().transform.localToWorldMatrix;
                Handles.color = Color.yellow;
                Handles.DrawLine(
                    GetMesh().vertices[i],
                    GetMesh().vertices[i] + GetMesh().normals[i]);
            }

            _verticesDone = true;
        }*/


        private void ShowNormalsAndTris()
        {
            // Debug.DrawRay(transform.position, transform.rotation.eulerAngles, Color.black, 10.0f);
            // selectedTris.Show();
            // MaterialHelper.GetFace(transform, GetMesh(), _selectedTrisIndex, md.OnSelectedMaterial);
        }
        


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object other)
        {
            print("called!");
            var item = other as SpNb0511;
            return item != null && this.name.Equals(item.name);
        }
        
        
        
    }

}