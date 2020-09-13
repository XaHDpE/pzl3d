using events;
using helpers;
using models.sp;
using UnityEngine;

namespace models.composite
{
    public class CompositeController : MonoBehaviour
    {
        public Camera cam;
        [SerializeField] private CustomEventsManager ec;
        public SparePartController target;
        public Material internalMaterial;
        private Mesh _mesh;

        private void OnEnable()
        {
            SparePartBase.SelectedNotification += OnSparePartSelected;
        }

        private void OnDisable()
        {
            SparePartBase.SelectedNotification -= OnSparePartSelected;
        }

        private void Update()
        {
            if (target == null) return;
            SnapToSurface();
        }

        private void OnSparePartSelected(Transform sp)
        {
            print($"spare part {sp.name} selected");
            target = sp.GetComponent<SparePartController>();
        }
    
        private void SnapToSurface()
        {
            var ray = cam.ScreenPointToRay(Input.mousePosition);
        
            if (!Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, 1 << 8)) return;
        
            var smUnderRay = hitInfo.transform.GetComponent<MeshFilter>().sharedMesh;
            var mrUnderRay = hitInfo.transform.GetComponent<MeshRenderer>();
            var subMeshIndex = MaterialHelper.GetSubMeshIndex(smUnderRay, hitInfo.triangleIndex);
            ec.OnSparePartUnderRayCast(hitInfo.transform, subMeshIndex);
        
            if (MaterialHelper.CheckMaterial(mrUnderRay, subMeshIndex, internalMaterial))
            {
                // var targetPos = hitInfo.point;
                // targetPos -= Vector3.back * target.GetComponent<Renderer>().bounds.extents.z;
                // var targetPos = target.transform.position;
                // Debug.DrawLine(hitInfo.point, targetPos, Color.green);
                // Debug.DrawRay(hitInfo.point, targetPos-hitInfo.point, Color.magenta);
                // var rey1 = new Ray(hitInfo.point, target.transform.position);
                target.transform.position = hitInfo.point;
                // target.transform.rotation = Quaternion.FromToRotation(Vector3.back, hitInfo.normal); 
            }

        }
    
    }
}