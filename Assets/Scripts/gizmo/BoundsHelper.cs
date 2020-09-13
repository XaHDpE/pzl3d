using System.Collections.Generic;
using UnityEngine;

namespace gizmo
{
    public class BoundsHelper
    {
        public static List<Vector3> Corners(GameObject go)
        {
            var boundPoint1 = go.GetComponent<Renderer>().bounds.min;
            var boundPoint2 = go.GetComponent<Renderer>().bounds.max;
            var boundPoint3 = new Vector3(boundPoint1.x, boundPoint1.y, boundPoint2.z);
            var boundPoint4 = new Vector3(boundPoint1.x, boundPoint2.y, boundPoint1.z);
            var boundPoint5 = new Vector3(boundPoint2.x, boundPoint1.y, boundPoint1.z);
            var boundPoint6 = new Vector3(boundPoint1.x, boundPoint2.y, boundPoint2.z);
            var boundPoint7 = new Vector3(boundPoint2.x, boundPoint1.y, boundPoint2.z);
            var boundPoint8 = new Vector3(boundPoint2.x, boundPoint2.y, boundPoint1.z);

            var corTemp = new List<Vector3>
            {
                boundPoint1,
                boundPoint2,
                boundPoint3,
                boundPoint4,
                boundPoint5,
                boundPoint6,
                boundPoint7,
                boundPoint8
            };
        
            // print(" ---> " + string.Join(",", corTemp));
        
            return corTemp;
        }
    
        public static Vector3[] Matrix(Transform what, int mCountX, int mCountY, int mCountZ)
        {
            // var locScale = what.localScale;
            var locScale = what.GetComponent<MeshRenderer>().bounds.size;
            var trPos = what.GetComponent<MeshRenderer>().transform.position;
        
            var startPoint = new Vector3(
                trPos.x - locScale.x / 2,
                trPos.y - locScale.y / 2, 
                trPos.z - locScale.z / 2
            );

            var counter = 0;
            var cellCenterX = (locScale.x / mCountX) / 2;
            var cellCenterY = (locScale.y / mCountY) / 2;
            var cellCenterZ = (locScale.z / mCountZ) / 2;
        
            // Gizmos.DrawSphere(startPoint, 0.3f);

            var mPositions = new Vector3[mCountX * mCountY * mCountZ];

            for (var i = 0; i < mCountX; i++) {
                for (var j = 0; j < mCountY; j++) {
                    for (var k = 0; k < mCountZ; k++) {
                        mPositions[counter].x = startPoint.x + (locScale.x / mCountX) * (i + 1) - cellCenterX;
                        mPositions[counter].y = startPoint.y + (locScale.y / mCountY) * (j + 1) - cellCenterY;
                        mPositions[counter].z = startPoint.z + (locScale.z / mCountZ) * (k + 1) - cellCenterZ;
                        counter++;
                    }
                }
            }

            return mPositions;
        }
    
    
    }
}