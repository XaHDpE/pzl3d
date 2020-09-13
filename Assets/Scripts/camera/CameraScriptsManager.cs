using events;
using Lean.Touch;
using UnityEngine;

public class CameraScriptsManager : MonoBehaviour
{
    public CustomEventsManager ec;
    private LeanPinchCamera lpc;
    private LeanPitchYaw lpy;
    private LeanMultiUpdate lmu;
    private LeanDragCamera ldc;

    // Start is called before the first frame update
    void Start()
    {
        lpc = Camera.main.GetComponent<LeanPinchCamera>();
        lpy = GetComponent<LeanPitchYaw>();
        lmu = GetComponent<LeanMultiUpdate>();
        ldc = GetComponent<LeanDragCamera>();
    }

    private void OnDestroy()
    {
        ec.SparePartSelected -= DisableCameraMovement;
        ec.NoneIsSelected -= EnableCameraMovement;
    }

    private void OnEnable()
    {
        ec.SparePartSelected += DisableCameraMovement;
        ec.NoneIsSelected += EnableCameraMovement;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void EnableCameraMovement()
    {
        print("enabling camera movement");
        lpc.enabled = true;
        lpy.enabled = true;
        lmu.enabled = true;
        ldc.enabled = true;
    }
    
    private void DisableCameraMovement()
    {
        print("disabling camera movement");
        lpc.enabled = false;
        lpy.enabled = false;
        lmu.enabled = false;
        ldc.enabled = false;
    }
    
}
