using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GizmoHelper : MonoBehaviour
{

    public Transform main;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(main.position, Vector3.up);
    }

    private void OnDrawGizmos()
    {
        if (main == null) return;
        // Draws a blue line from this transform to the target
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, main.position);
    }
    

}
