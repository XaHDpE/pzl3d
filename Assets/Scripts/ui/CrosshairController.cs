using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    public Camera cam;
    
    // Start is called before the first frame update
    private void Start()
    {
        var pos = cam.ViewportToWorldPoint(
            new Vector3(0.5f, 0.5f, cam.nearClipPlane + GetComponent<Renderer>().bounds.size.x)
            );
        transform.position = pos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
