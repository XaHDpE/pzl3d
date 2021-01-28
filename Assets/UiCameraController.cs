using System;
using camera;
using helpers;
using UnityEngine;

public class UiCameraController : MonoBehaviour
{
    // Start is called before the first frame update

    private void OnEnable()
    {
        CameraController.CarouselCameraSet += SyncPosition;
    }
    
    private void OnDisable()
    {
        CameraController.CarouselCameraSet -= SyncPosition;
    }

    private void SyncPosition(Camera cam, Vector3 center, float zsize)
    {
        transform.position = cam.transform.position;
        transform.rotation = cam.transform.rotation;
    }
    
}
