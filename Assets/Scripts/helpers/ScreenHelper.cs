using System.Linq;
using UnityEngine;

namespace helpers
{
    public class ScreenHelper : MonoBehaviour
    {
    
        public static ScreenHelper instance = null;

        public ScreenHelper()
        {
        }

        private void Start()
        {
            if (instance == null) {
                instance = this;
            } else if(instance == this){ 
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
            InitializeManager();
        }
    
        private void InitializeManager(){
        }

        public static GameObject GetGameObjectByRay(Ray ray)
        {
            if (!Physics.Raycast(ray, out var hit)) return null;
            var tappedObject = hit.collider.gameObject;
            return tappedObject != null ? tappedObject : null;
        }

        public void MoveCamToFitObject(Camera cam, Transform what)
        {
            const float margin = 1.1f;
        
            // var extentsTmp = what.GetComponent<Renderer>().bounds.extents;
            // Debug.Log("extents:" + extentsTmp);
            // var extents = extentsTmp.Equals(Vector3.zero) ? GetBounds(what.gameObject).extents : extentsTmp;
            var maxExtent = GetBounds1(what.gameObject).extents.magnitude;
            var minDistance = (maxExtent * margin) / Mathf.Sin(Mathf.Deg2Rad * cam.fieldOfView / 2.0f);
            cam.transform.position = Vector3.back * minDistance;
            cam.nearClipPlane = minDistance - maxExtent;
        }
    
        public void Scale(Transform what, Camera cam, float relativeSizeY)
        {
            var position = what.position;
            var targetSize = Vector3.Distance(
                cam.ViewportToWorldPoint(new Vector3(0f, 0f, 10f)),
                cam.ViewportToWorldPoint(new Vector3(0f, relativeSizeY, 10f)));
        
            Debug.Log("what.targetSize: " + targetSize);
        
            var boundsSize = what.GetComponent<Renderer>().bounds.size;
            Debug.Log("rO:" + boundsSize);
            var currentSize = boundsSize.Equals(Vector3.zero) ? GetBounds(what.gameObject).size : boundsSize;

            var currScale = what.transform.localScale;
        
            Debug.Log("targetSize: " + targetSize + 
                      ", currScale: " + currScale +
                      ", currentSize: " + currentSize);
        
        
            currScale = new Vector3(
                targetSize * currScale.x / currentSize.y,
                targetSize * currScale.y / currentSize.y,
                targetSize * currScale.z / currentSize.y
            );
            what.localScale = currScale;
        
            Debug.Log(what.name + " : " + currScale);
        }

        /*
    public Vector3 GetViewPortMenuCoordinate(Camera cam, int iterator)
    {
        return cam.ViewportToWorldPoint(
            new Vector3(
                SettingsReader.Instance.GameSettings.DistanceBetweenMenuItems * iterator, 
                0.1f, 
                SettingsReader.Instance.GameSettings.SparePartSelectorDepth));
    }
    */

        private void SetToCenter(Transform what, Camera cam)
        {
            var z = cam.transform.InverseTransformPoint(what.position).z;
            var centerPos = cam.ViewportToWorldPoint(
                new Vector3(0.5f, 0.5f, z)
            );
            what.position = centerPos;
        }

        public void LockObjectInViewPort(Transform what, Camera cam)
        {
            var dist = cam.transform.InverseTransformPoint(what.position).z;
            var localPos = cam.transform.InverseTransformPoint(what.position);
            var leftBottom = cam.ViewportToWorldPoint(new Vector3(0, 0, dist));
            var rightTop = cam.ViewportToWorldPoint(new Vector3(1, 1, dist));
            leftBottom = cam.transform.InverseTransformPoint(leftBottom);
            rightTop = cam.transform.InverseTransformPoint(rightTop);

            Debug.Log("leftBottom: " + leftBottom);
            Debug.Log("rightTop: " + rightTop);

            var x = Mathf.Clamp(localPos.x, leftBottom.x, rightTop.x);
            var y = Mathf.Clamp(localPos.y, leftBottom.y, rightTop.y);

            Debug.Log("x: " + x);
            Debug.Log("y: " + y);

            what.transform.position = cam.transform.TransformPoint(new Vector3(x, y, localPos.z));
        }

        public void SetCompositeToViewPortCenter(Transform what)
        {
            SetToCenter(
                what,
                Camera.main
            );
            //TODO temporary solution, need to rewrite later
            var trf = what.transform;
            trf.position += new Vector3(0, 0, 12.0f);
            // what.RotateAround(what.position, trf.up, 230);
        }

        public Bounds GetBounds1(GameObject obj)
        {
            // First find a center for your bounds.
            var center = obj.transform.Cast<Transform>().Aggregate(
                Vector3.zero, 
                (current, child) => current + current
            );
            center /= obj.transform.childCount; //center is average center of children

            //Now you have a center, calculate the bounds by creating a zero sized 'Bounds', 
            var bounds = new Bounds(center,Vector3.zero);
            foreach (Transform child in obj.transform)
            {
                if (child.gameObject.CompareTag("Lean")) continue;
                bounds.Encapsulate(child.GetComponent<Renderer>().bounds);   
            }
            return bounds;
        }

        public Bounds GetBounds(GameObject obj)
        {
            var rr = obj.GetComponentsInChildren<Renderer>();
            var b = rr[0].bounds;
            foreach (var r in rr)
            {
                b.Encapsulate(r.bounds);
            }
            return b;
        }
    
    }
}