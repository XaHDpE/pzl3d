using UnityEngine;

namespace gizmo
{
    [ExecuteAlways]
    public class ShowMeshBounds : MonoBehaviour {
        public Color color = Color.green;
     
        [SerializeField]
        private Vector3 v3FrontTopLeft;
        [SerializeField]
        private Vector3 v3FrontTopRight;
        [SerializeField]
        private Vector3 v3FrontBottomLeft;
        [SerializeField]
        private Vector3 v3FrontBottomRight;
        [SerializeField]
        private Vector3 v3BackTopLeft;
        [SerializeField]
        private Vector3 v3BackTopRight;
        [SerializeField]
        private Vector3 v3BackBottomLeft;
        [SerializeField]
        private Vector3 v3BackBottomRight;

        private void Update() {
            CalcPositons();
            DrawBox();
        }

        private Bounds CalculateLocalBounds()
        {
                var currentRotation = transform.rotation;
                transform.rotation = Quaternion.Euler(0f,0f,0f);
                var bounds = new Bounds(transform.position, Vector3.zero);
                foreach(var rend in GetComponentsInChildren<Renderer>())
                {
                    bounds.Encapsulate(rend.bounds);
                }
                var localCenter = bounds.center - transform.position;
                bounds.center = localCenter;
                transform.rotation = currentRotation;
                return bounds;
        }
        
        private void CalcPositons(){
            // var bounds = GetComponent<MeshFilter>().sharedMesh.bounds;
            var bounds = CalculateLocalBounds();
            // var bounds = GetLocalBoundsForObject(transform.gameObject);
         
            //Bounds bounds;
            //BoxCollider bc = GetComponent<BoxCollider>();
            //if (bc != null)
            //    bounds = bc.bounds;
            //else
            //return;
         
            var v3Center = bounds.center;
            var v3Extents = bounds.extents;
  
            v3FrontTopLeft     = new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z);  // Front top left corner
            v3FrontTopRight    = new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z);  // Front top right corner
            v3FrontBottomLeft  = new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z);  // Front bottom left corner
            v3FrontBottomRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z);  // Front bottom right corner
            v3BackTopLeft      = new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z + v3Extents.z);  // Back top left corner
            v3BackTopRight     = new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z + v3Extents.z);  // Back top right corner
            v3BackBottomLeft   = new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z + v3Extents.z);  // Back bottom left corner
            v3BackBottomRight  = new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z + v3Extents.z);  // Back bottom right corner
         
            v3FrontTopLeft     = transform.TransformPoint(v3FrontTopLeft);
            v3FrontTopRight    = transform.TransformPoint(v3FrontTopRight);
            v3FrontBottomLeft  = transform.TransformPoint(v3FrontBottomLeft);
            v3FrontBottomRight = transform.TransformPoint(v3FrontBottomRight);
            v3BackTopLeft      = transform.TransformPoint(v3BackTopLeft);
            v3BackTopRight     = transform.TransformPoint(v3BackTopRight);
            v3BackBottomLeft   = transform.TransformPoint(v3BackBottomLeft);
            v3BackBottomRight  = transform.TransformPoint(v3BackBottomRight);    
        }

        private void DrawBox() {
            Debug.DrawLine (v3FrontTopLeft, v3FrontTopRight, color);
            Debug.DrawLine (v3FrontTopRight, v3FrontBottomRight, color);
            Debug.DrawLine (v3FrontBottomRight, v3FrontBottomLeft, color);
            Debug.DrawLine (v3FrontBottomLeft, v3FrontTopLeft, color);
         
            Debug.DrawLine (v3BackTopLeft, v3BackTopRight, color);
            Debug.DrawLine (v3BackTopRight, v3BackBottomRight, color);
            Debug.DrawLine (v3BackBottomRight, v3BackBottomLeft, color);
            Debug.DrawLine (v3BackBottomLeft, v3BackTopLeft, color);
         
            Debug.DrawLine (v3FrontTopLeft, v3BackTopLeft, color);
            Debug.DrawLine (v3FrontTopRight, v3BackTopRight, color);
            Debug.DrawLine (v3FrontBottomRight, v3BackBottomRight, color);
            Debug.DrawLine (v3FrontBottomLeft, v3BackBottomLeft, color);
        }
        
        static Bounds GetLocalBoundsForObject(GameObject go)
        {
            var referenceTransform = go.transform;
            var b = new Bounds(Vector3.zero, Vector3.zero);
            RecurseEncapsulate(referenceTransform, ref b);
            return b;
                       
            void RecurseEncapsulate(Transform child, ref Bounds bounds)
            {
                var mesh = child.GetComponent<MeshFilter>();
                if (mesh)
                {
                    var lsBounds = mesh.sharedMesh.bounds;
                    var wsMin = child.TransformPoint(lsBounds.center - lsBounds.extents);
                    var wsMax = child.TransformPoint(lsBounds.center + lsBounds.extents);
                    bounds.Encapsulate(referenceTransform.InverseTransformPoint(wsMin));
                    bounds.Encapsulate(referenceTransform.InverseTransformPoint(wsMax));
                }
                foreach (Transform grandChild in child.transform)
                {
                    RecurseEncapsulate(grandChild, ref bounds);
                }
            }
        }
     
    }
}