using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void MovePlaneAlong(Vector3 position, Vector3 trisNormal)
    {
        var rotationIncrement = Quaternion.FromToRotation(transform.TransformVector(transform.forward), trisNormal);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + rotationIncrement.eulerAngles);
        transform.position = position;
        
    }
    
}
