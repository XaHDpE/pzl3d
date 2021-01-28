using System;
using helpers;
using UnityEngine;

public class AgainTest : MonoBehaviour
{
    private void Start()
    {
        
    }

    private void OnDrawGizmos()
    {
        var totalBound = TransformHelper.GetHierarchicalBounds(gameObject);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(totalBound.center,totalBound.size);
    }
}
