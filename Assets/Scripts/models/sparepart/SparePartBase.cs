using System;
using Lean.Touch;
using UnityEngine;

namespace models.sparepart
{
    [RequireComponent(typeof(MeshCollider))]
    [RequireComponent(typeof(LeanSelectable))]
    public abstract class SparePartBase : MonoBehaviour, ISparePart
    {
        private LeanSelectable _ls;
        private Mesh _mesh;
        private MeshRenderer _mr;
        private MeshCollider _collider;
        
        private Camera _mainCamera;
        private Camera _currentCamera;
        private bool _selectionPropagationFlag;

        // events
        public static event Action<Transform> SelectedNotification = (bc) => { };
        public static event Action DeselectedNotification = () => { };
        
        protected static void InvokeSparePartSelectedEvent(Transform classInstance)
        {
            SelectedNotification?.Invoke(classInstance);
        }

        protected static void InvokeSparePartDeselectedEvent()
        {
            DeselectedNotification?.Invoke();
        }

        protected void PropagateSelectionToParent(bool val)
        {
            _selectionPropagationFlag = val;
        }

        public abstract Transform GetSparePart();

        protected virtual void OnSelectHandler(LeanFinger finger)
        {
            Debug.Log("base.OnSelectHandler executed.");
            if (_selectionPropagationFlag) transform.parent.GetComponent<LeanSelectable>().IsSelected = true;
        }

        protected virtual void OnDeSelectHandler()
        {
            Debug.Log("base.OnDeSelectHandler executed.");
            if (_selectionPropagationFlag) transform.parent.GetComponent<LeanSelectable>().IsSelected = false;
        }

        protected virtual void OnEnable()
        {
            RegisterBaseListeners();
            RegisterBaseComponents();
        }
        
        protected virtual void OnDisable()
        {
            UnregisterBaseListeners();
        }

        private void RegisterBaseListeners()
        {
            _ls.OnSelect.AddListener(OnSelectHandler);
            _ls.OnDeselect.AddListener(OnDeSelectHandler);
        }

        private void UnregisterBaseListeners()
        {
            _ls.OnSelect.RemoveAllListeners();
            _ls.OnDeselect.RemoveAllListeners();
        }
        
        private void RegisterBaseComponents()
        {
            _mesh = GetComponent<MeshFilter>().sharedMesh;
            _mr = GetComponent<MeshRenderer>();
            
            _mainCamera = Camera.main; 
            
            // register lean scripts
            _ls = GetComponent<LeanSelectable>();

            _collider = GetComponent<MeshCollider>();
            _collider.convex = false;
        }

        protected virtual void Awake()
        {
            
            RegisterBaseComponents();
        }

        protected Camera GetMainCamera()
        {
            return _mainCamera;
        }

        protected MeshRenderer GetMeshRenderer()
        {
            return _mr;
        }

        public Mesh GetMesh()
        {
            return _mesh;
        }

        protected LeanSelectable GetLeanSelectable()
        {
            return _ls;
        }
        
        public Camera CurrentCamera
        {
            get => _currentCamera;
            set => _currentCamera = value;
        }
    }
}