using models;
using UnityEngine;

namespace helpers
{
    public static class CollidersHelper
    {
        public static void GenerateColliderByEdge(Transform parent, EdgeVector edge)
        {
            var goCol = new GameObject("coll");
            goCol.transform.SetParent(parent);
            var boxCollider = goCol.AddComponent<BoxCollider>();
            var scaledCoord1 = parent.TransformPoint(edge.Coord1);
            var scaledCoord2 = parent.TransformPoint(edge.Coord2);
            
            // position
            var tmpPos = (scaledCoord1 + scaledCoord2) / 2;
            var boxTransform = goCol.transform;
            boxTransform.position = tmpPos;

            // scale
            var tmpScale = goCol.transform.localScale;
            tmpScale.x = Vector3.Distance(edge.Coord1, edge.Coord2);
            tmpScale.y = 0.01f;
            tmpScale.z = 0.01f;
            goCol.transform.localScale = tmpScale;
            
            // rotation
            boxCollider.transform.rotation = Quaternion.FromToRotation(
                boxTransform.right,
                scaledCoord1 - scaledCoord2
            ); 
        }
    }
}