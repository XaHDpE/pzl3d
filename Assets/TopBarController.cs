using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using camera;
using helpers;
using states.controllers;
using states.sparepart;
using UnityEngine;

public class TopBarController : MonoBehaviour
{
    // public Camera cam;
    public List<Transform> children;
    public delegate void TopBarRearrangedDelegate();
    public static event TopBarRearrangedDelegate TopBarRearranged;
    
    private Bounds _bounds;
    
    

    private void OnEnable()
    {
        CameraController.CarouselCameraSet += PlaceContainer;
        CarouselItemSelectedState.NewIsMovingInTopBar += ProcessSwipeUp;
    }

    private void OnDisable()
    {
        CameraController.CarouselCameraSet -= PlaceContainer;
        CarouselItemSelectedState.NewIsMovingInTopBar -= ProcessSwipeUp;
    }

    private void PlaceContainer(Camera leadCam, Vector3 center, float zSize)
    {
        gameObject.layer = LayerMask.NameToLayer("Green");
        var leadCamTransform = leadCam.transform;
        
        var distance = Vector3.Distance(leadCamTransform.position, center - leadCamTransform.forward * zSize/2);
        
        // var distance = Vector3.Distance(cam.transform.position , gameObject.transform.position);
        var frustumHeight =  2.0f * distance * Mathf.Tan(leadCam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        var frustumWidth = frustumHeight * Screen.width / Screen.height;

        // 100% of frustum width
        var xScale = frustumWidth;
        // 20% of frustum height
        var yScale = frustumHeight / 5f;
        var zScale = yScale;

        // move to top center of the viewport
        var newPos = leadCam.ViewportToWorldPoint(new Vector3(0.5f, 1f, distance));
        // move down to entirely fit the container
        var transform1 = transform;
        newPos -= transform1.up * yScale / 2;
        // go forward in the direction of carousel cam
        newPos += leadCam.transform.forward * zScale / 2;

        transform1.position = newPos;
        transform1.rotation = leadCamTransform.rotation;
        transform1.localScale = new Vector3(xScale, yScale, zScale);

        // upscaled bounds
        //_bounds = transform.GetComponent<Renderer>().bounds;
    }

    private void RecalculatePositions()
    {
        _bounds = transform.GetComponent<Renderer>().bounds;
        
        // get children and remove parent from the list
        children = transform.GetComponentsInChildren<Transform>().ToList();
        children.RemoveAt(0);

        var movers = new IEnumerator[children.Count];
        
        var targetSize = GetComponent<Renderer>().bounds.size.y;
        
        for (var index = 0; index < children.Count; index++)
        {
            var ci = children[index];
            var curXOffset = (2 * (index) + 1) * _bounds.size.x / (2 * children.Count) - _bounds.extents.x;
            var newScale = targetSize * ci.localScale / ci.GetComponent<Renderer>().bounds.size.magnitude;

            var tr = transform;
            movers[index] = TransformHelper.MoveScale(
                this, ci, tr.position + tr.right * curXOffset, newScale, 0.3f
                );
        }

        StartCoroutine(UpdatePositions(movers, () => TopBarRearranged?.Invoke()));

    }

    private IEnumerator UpdatePositions(IEnumerator[] movers, Action callback)
    {
        var cors = movers.Select(StartCoroutine).ToList();
        foreach (var cor in cors)
        {
            yield return cor;
        }
        callback();
    }

    private void ProcessSwipeUp(InputManagerController newItem)
    {
        // update state
        newItem.MoveToSelectedInTopBarState();
        // change parent of the former carousel item
        newItem.transform.SetParent(transform);
        // recalculate top bar elements positions and scales
        RecalculatePositions();
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        other.transform.SetParent(transform);
        RecalculatePositions();
    }
    */
    

}
