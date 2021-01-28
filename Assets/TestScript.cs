using System;
using System.Collections;
using helpers;
using UnityEngine;

public class TestScript : MonoBehaviour
{

    private Camera _mainCam;
    private float _dst;
    private Bounds objectBounds;
    
    // Start is called before the first frame update
    void Start1()
    {
        // var newPos = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1f, 30f));
        // TransformHelper.MoveTo(transform, newPos, 3f);
        _mainCam = Camera.main;
        var dist = Vector3.Distance(transform.position,_mainCam.transform.position);
        // _mainCam.GetComponent<CameraController>().LookAt(transform);
        objectBounds = GetComponent<MeshRenderer>().bounds;
    }

    private void HalfSize(float distance, float viewPortPart)
    {
        var height = 2.0 * Mathf.Tan(0.5f * _mainCam.fieldOfView * Mathf.Deg2Rad) * distance;
        var width = height * Screen.width / Screen.height;
        
        var targetSize = (float) Math.Min(height, width);
        
        Debug.Log($"height: {height}, width: {width}, {Camera.main.fieldOfView}");
        
        var currentSize = TransformHelper.GetHierarchicalBounds(gameObject).extents.magnitude;
        var currScale = transform.localScale;
        
        Debug.Log($"targetSize: {targetSize}, currScale: " +
                  $"{currScale}, currentSize: {currentSize}, " +
                  $"{targetSize * currScale.x / currentSize}");
        
        /*

        var newScale = new Vector3(
            targetSize * currScale.x / currentSize,
            targetSize * currScale.y / currentSize,
            targetSize * currScale.z / currentSize
        );

        var scaleTo = TransformHelper.ScaleTo(transform, newScale, 5);

        var exts = GetBounds(gameObject).extents;
        Debug.Log($"newScale: {newScale}, finalSize: {exts.x}, {exts.y}, {exts.z}");
        */
    }

    public void ShowDigits()
    {
        Debug.Log($"curExtents: {TransformHelper.GetHierarchicalBounds(gameObject).extents}");
    }

    public void AddBox()
    {
        var newObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        newObj.transform.SetParent(transform);
        newObj.transform.localPosition += new Vector3(objectBounds.size.magnitude, 0, 0);
        Debug.Log($"new pos: {newObj.transform.localPosition}");
    }

    public float elevation;
    public float cameraDistance = 1.0f;

    private void FixCamera()
    {
        var objectSizes = objectBounds.max - objectBounds.min;
        var objectSize = Mathf.Max(objectSizes.x, objectSizes.y, objectSizes.z);
        var cameraView = 2.0f * Mathf.Tan(0.5f * Mathf.Deg2Rad * Camera.main.fieldOfView);
        var distance = cameraDistance * objectSize / cameraView;
        distance += 0.5f * objectSize;
        var center = 
        _mainCam.transform.position = objectBounds.center - distance * _mainCam.transform.forward;
        _mainCam.transform.rotation = Quaternion.Euler(new Vector3(elevation, 0, 0));
    }

    private void OnDrawGizmos()
    {
        // Gizmos.DrawWireSphere(objectBounds.center, 1.0f);
    }

    private void Start11()
    {
        var firstItem = transform.GetComponentsInChildren<Transform>()[1];
			
        var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.transform.position = firstItem.position;
        go.transform.localScale = new Vector3(3,3,3);
			
        var go1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go1.transform.SetParent(transform);
        go1.transform.localPosition = firstItem.localPosition;
        go1.transform.localScale = new Vector3(3,3,3);
    }

    private Coroutine moveRoutine;

    private void Start2()
    {
        _mainCam = Camera.main;
    }

    private void Update1()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            var nv = _mainCam.ViewportToWorldPoint(new Vector3(0.5f,1, 30));
            Move(transform, nv, new Vector3(7,7,7), 1);
        }
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            var nv = _mainCam.ViewportToWorldPoint(new Vector3(0.5f,0, 30));
            Move(transform, nv, new Vector3(2,2,2), 1);
        } 
        
        /*if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log($"{KeyCode.E} pressed");
            TransformHelper.ScaleTo(transform, new Vector3(2, 2, 2), 50, cts.Token);
        }*/
        
        
    }

    private void Move(Transform what, Vector3 where, Vector3 targetScale, float timeTakes)
    {
        if (moveRoutine != null)
            StopCoroutine(moveRoutine);
        moveRoutine = StartCoroutine(TransformHelper.MoveScale(this, what, where, targetScale, timeTakes));
    }


    public void Go()
    {
        Debug.Log("go");
        var wantedRotation = Quaternion.Euler(transform.rotation.eulerAngles.x + 45,0,0);
        var c = TransformHelper.RotateTo(transform, wantedRotation, 45);
        StartCoroutine(c);
    }

    
}
