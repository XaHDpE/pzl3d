using System;
using UnityEngine;

public class FrustrumTest : MonoBehaviour
{

    public Transform target;


    private void Update()
    {
        if (target == null) return;
        Do();
    }

    private void Do()
    {
        // Calculate the planes from the main camera's view frustum
        var planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

        // Create a "Plane" GameObject aligned to each of the calculated planes
        foreach (var t in planes)
        {
            if (GeometryUtility.TestPlanesAABB(new Plane[] {t}, target.GetComponent<MeshFilter>().sharedMesh.bounds))
            {
                Debug.Log(target.name + $" has been detected, plane: {t}");
            }
            else
            {
                Debug.Log("Nothing has been detected");
            }
        }
    }
}
