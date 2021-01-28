using System;
using input;
using UnityEngine;

namespace helpers
{
    public class ArrangementHelper
    {
        public Tuple<Transform, Vector3>[] ArrangeInCircle(Transform[] items, Vector3 centerPos, float radius, ref float deltaAngle)
        {
            var result =new Tuple<Transform, Vector3>[items.Length];
            deltaAngle = 360 % items.Length;
            
            for (var i = 0; i < items.Length; i++)
            {
                var angle = i * Mathf.PI * 2 / items.Length;
                var x = Mathf.Sin(angle) * radius;
                var z = Mathf.Cos(angle) * radius;
                var pos = centerPos + new Vector3(x, 0, z);
                var angleDegrees = -angle * Mathf.Rad2Deg;
                var rot = Quaternion.Euler(0, angleDegrees, 0);    
            }

            return result;
        }
    }
}